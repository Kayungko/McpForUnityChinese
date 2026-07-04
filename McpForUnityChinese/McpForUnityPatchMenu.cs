#if UNITY_EDITOR
using UnityEditor;

namespace IdelGame.Editor.ThirdPartyPatches.McpForUnityChinese
{
    /// <summary>
    /// "MCP for Unity" 源码级汉化补丁的手动入口：强制重新应用 / 还原英文原版。
    /// 日常无需手动操作——补丁会在每次 Editor 加载时自动检测应用。
    /// </summary>
    internal static class McpForUnityPatchMenu
    {
        [MenuItem("Tools/汉化/立即应用 MCP for Unity 中文补丁", false)]
        private static void ApplyNow()
        {
            McpForUnitySourcePatcher.ApplyPatch(force: true);
        }

        [MenuItem("Tools/汉化/还原 MCP for Unity 英文原版", false)]
        private static void RestoreNow()
        {
            McpForUnitySourcePatcher.RestorePatch();
        }
    }
}
#endif
