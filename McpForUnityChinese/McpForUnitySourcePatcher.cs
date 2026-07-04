#if UNITY_EDITOR
using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using PackageInfo = UnityEditor.PackageManager.PackageInfo;

namespace IdelGame.Editor.ThirdPartyPatches.McpForUnityChinese
{
    /// <summary>
    /// "MCP for Unity"（com.coplaydev.unity-mcp）源码级汉化补丁引擎。
    /// 直接对包解析后的物理文件（Library/PackageCache/... 或其他 resolve 方式得到的 resolvedPath）
    /// 下 Editor/Windows 子树的 .uxml / .cs 做文本级替换，不依赖运行时轮询。
    /// 自包含：只用 UnityEditor / UnityEditor.PackageManager / System.IO / System.Text.RegularExpressions，
    /// 可直接复制到其他项目的 Editor 目录下独立工作。
    /// </summary>
    [InitializeOnLoad]
    internal static class McpForUnitySourcePatcher
    {
        private const string WindowsSubFolder = "Editor/Windows";
        private const string MarkerFileName = ".mcp-zh-patched";
        private const string BackupFolderName = ".mcp-zh-backup";

        // UXML 属性值：只在 text="..." / tooltip="..." 属性上下文里替换，避免误改其他属性。
        private static readonly Regex UxmlAttributePattern = new Regex(
            @"(?<prefix>\btext=""|\btooltip="")(?<value>[^""]*)""",
            RegexOptions.Compiled);

        // C# 字符串字面量：单行双引号包裹的内容（覆盖直接赋值、三元表达式等写法）。
        // 只在包含 .text / .tooltip 的行上应用，且只有内容精确命中字典才替换，避免误伤同一文件里其他无关字符串。
        private static readonly Regex CsStringLiteralPattern = new Regex(
            "\"(?<value>[^\"\\n]*)\"",
            RegexOptions.Compiled);

        // 逐行按 \r\n / \r / \n 切分，奇数下标是被捕获的换行符本身，用于原样拼回，不破坏文件的换行风格。
        private static readonly Regex LineSplitPattern = new Regex("(\r\n|\r|\n)", RegexOptions.Compiled);

        static McpForUnitySourcePatcher()
        {
            // 每次 Editor 加载/重新编译都尝试一次：内部已做幂等 + 版本校验，开销极小；
            // Library 被清理、全新 clone、包因跟踪 #main 被 resolve 到新 commit 时都能自愈。
            ApplyPatch(force: false);
        }

        internal static bool TryFindWindowsFolder(out string windowsFolder, out string version)
        {
            windowsFolder = null;
            version = null;

            var packageInfo = PackageInfo.GetAllRegisteredPackages()
                .FirstOrDefault(p => p.name == McpForUnityPatchData.PackageName);
            if (packageInfo == null || string.IsNullOrEmpty(packageInfo.resolvedPath))
            {
                return false;
            }

            version = packageInfo.version;
            windowsFolder = Path.Combine(packageInfo.resolvedPath, WindowsSubFolder);
            return Directory.Exists(windowsFolder);
        }

        internal static void ApplyPatch(bool force)
        {
            if (!TryFindWindowsFolder(out var windowsFolder, out var version))
            {
                return;
            }

            if (!McpForUnityPatchData.SupportedVersions.Contains(version))
            {
                Debug.LogWarning(
                    $"[MCP for Unity 汉化补丁] 检测到包版本 {version}，未在已验证列表中（{string.Join(", ", McpForUnityPatchData.SupportedVersions)}），本次跳过，保留英文原文。");
                return;
            }

            string markerPath = Path.Combine(windowsFolder, MarkerFileName);
            if (!force && File.Exists(markerPath) && File.ReadAllText(markerPath).Trim() == version)
            {
                return;
            }

            string backupFolder = Path.Combine(windowsFolder, BackupFolderName);
            bool anyChanged = false;

            // ToList() 立即固化文件清单：Directory.EnumerateFiles 是惰性流式枚举，若不固化，
            // 循环内刚创建的 backupFolder 会被同一次递归枚举实时发现并当成待翻译文件误处理，
            // 导致备份文件被反向翻译成中文（RestorePatch 因此失效）。同时显式排除备份目录本身。
            var files = Directory.EnumerateFiles(windowsFolder, "*.uxml", SearchOption.AllDirectories)
                .Concat(Directory.EnumerateFiles(windowsFolder, "*.cs", SearchOption.AllDirectories))
                .Where(f => !f.StartsWith(backupFolder, StringComparison.OrdinalIgnoreCase))
                .ToList();

            foreach (var filePath in files)
            {
                string original = File.ReadAllText(filePath);
                bool isUxml = filePath.EndsWith(".uxml", StringComparison.OrdinalIgnoreCase);
                string patched = ApplyTextReplacements(original, isUxml);

                if (patched == original)
                {
                    continue;
                }

                BackupOriginalIfNeeded(windowsFolder, backupFolder, filePath, original);
                File.WriteAllText(filePath, patched);
                anyChanged = true;
            }

            if (anyChanged || force)
            {
                File.WriteAllText(markerPath, version);
                AssetDatabase.Refresh();
                Debug.Log($"[MCP for Unity 汉化补丁] 已对版本 {version} 应用中文补丁。");
            }
        }

        internal static void RestorePatch()
        {
            if (!TryFindWindowsFolder(out var windowsFolder, out _))
            {
                return;
            }

            string backupFolder = Path.Combine(windowsFolder, BackupFolderName);
            if (!Directory.Exists(backupFolder))
            {
                Debug.LogWarning("[MCP for Unity 汉化补丁] 没有找到备份，无法还原。");
                return;
            }

            foreach (var backupFile in Directory.EnumerateFiles(backupFolder, "*", SearchOption.AllDirectories))
            {
                string relativePath = Path.GetRelativePath(backupFolder, backupFile);
                string targetPath = Path.Combine(windowsFolder, relativePath);
                File.Copy(backupFile, targetPath, overwrite: true);
            }

            Directory.Delete(backupFolder, recursive: true);

            string markerPath = Path.Combine(windowsFolder, MarkerFileName);
            if (File.Exists(markerPath))
            {
                File.Delete(markerPath);
            }

            AssetDatabase.Refresh();
            Debug.Log("[MCP for Unity 汉化补丁] 已还原为英文原版。");
        }

        private static void BackupOriginalIfNeeded(string windowsFolder, string backupFolder, string filePath, string originalContent)
        {
            string relativePath = Path.GetRelativePath(windowsFolder, filePath);
            string backupPath = Path.Combine(backupFolder, relativePath);

            if (File.Exists(backupPath))
            {
                return;
            }

            Directory.CreateDirectory(Path.GetDirectoryName(backupPath)!);
            File.WriteAllText(backupPath, originalContent);
        }

        private static string ApplyTextReplacements(string content, bool isUxml)
        {
            foreach (var (pattern, replacement) in McpForUnityPatchData.InterpolatedTemplates)
            {
                content = pattern.Replace(content, replacement);
            }

            if (isUxml)
            {
                return UxmlAttributePattern.Replace(content, ReplaceMatchIfKnown);
            }

            // C# 文件：只在包含 .text / .tooltip 的行上做字符串字面量替换，避免误伤同一文件里其他无关字符串。
            // 触发行若同时含 "switch"（如 `label.text = level switch`），说明后续是跨行 switch 表达式，
            // 持续放行直到出现以 "};" 收尾的行为止，否则 case 分支的字符串（不在触发行上）永远匹配不到。
            string[] segments = LineSplitPattern.Split(content);
            bool insideSwitchBlock = false;
            for (int i = 0; i < segments.Length; i += 2)
            {
                string line = segments[i];
                bool triggersOnThisLine = line.IndexOf(".text", StringComparison.Ordinal) >= 0 ||
                    line.IndexOf(".tooltip", StringComparison.Ordinal) >= 0;

                if (triggersOnThisLine && line.Contains("switch"))
                {
                    insideSwitchBlock = true;
                }

                if (!triggersOnThisLine && !insideSwitchBlock)
                {
                    continue;
                }

                segments[i] = CsStringLiteralPattern.Replace(line, ReplaceMatchIfKnown);

                if (insideSwitchBlock && line.TrimEnd().EndsWith("};", StringComparison.Ordinal))
                {
                    insideSwitchBlock = false;
                }
            }

            return string.Concat(segments);
        }

        private static string ReplaceMatchIfKnown(Match match)
        {
            string value = match.Groups["value"].Value;
            if (!McpForUnityPatchData.TextMap.TryGetValue(value, out var translated))
            {
                return match.Value;
            }

            Group prefixGroup = match.Groups["prefix"];
            string prefix = prefixGroup.Success ? prefixGroup.Value : "\"";
            return prefix + translated + "\"";
        }
    }
}
#endif
