#if UNITY_EDITOR
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace IdelGame.Editor.ThirdPartyPatches.McpForUnityChinese
{
    /// <summary>
    /// "MCP for Unity"（com.coplaydev.unity-mcp）源码级汉化补丁的翻译数据。
    /// 只包含数据，不含任何文件读写/正则替换逻辑（逻辑在 McpForUnitySourcePatcher 里）。
    /// 上游改了措辞导致某条译文失效时，直接在这里补一条新的映射即可，不用改补丁引擎代码。
    /// 版本校验按主版本号前缀匹配（见 SupportedVersions 上的说明）：主版本号不变时（如 10.0.x -> 10.1.x）
    /// 补丁会自动继续生效，只是新增/改名的文案在未补充字典前会保留英文；建议仍按上游发版节奏核对差异、
    /// 扩充条目，只有主版本号真正跳变（如 10.x -> 11.0.0）时才必须在 SupportedVersions 里追加新条目。
    /// </summary>
    internal static class McpForUnityPatchData
    {
        public const string PackageName = "com.coplaydev.unity-mcp";

        /// <summary>
        /// 已核对过、确认补丁可以安全应用的具体包版本。
        /// 校验规则（见 <see cref="McpForUnitySourcePatcher.IsVersionSupported"/>）按"主版本号前缀"匹配：
        /// 只要当前包版本的主版本号（第一个 "." 之前的数字）与本列表中任意一条相同，就视为兼容并应用补丁，
        /// 不要求逐字节匹配完整版本号（用于兼容包持续跟踪 upstream main/beta 分支、版本号随 commit 漂移的场景）。
        /// 主版本号发生变化（如 10.x -> 11.0.0）时才需要在这里追加一条新的具体版本号。
        /// </summary>
        public static readonly string[] SupportedVersions = { "10.0.0", "10.1.1-beta.1" };

        /// <summary>
        /// 精确字符串字典：英文原文 -> 中文译文。
        /// 应用范围仅限 "<c>.text = "..."</c>" / "<c>.tooltip = "..."</c>"（C#）
        /// 与 "<c>text="..."</c>" / "<c>tooltip="..."</c>"（UXML）这几种赋值上下文，避免误改非 UI 文本。
        /// </summary>
        public static readonly Dictionary<string, string> TextMap = new Dictionary<string, string>
        {
            // 顶部 Tab
            ["Connect"] = "连接",
            ["Tools"] = "工具",
            ["Resources"] = "资源",
            ["Asset Gen"] = "AI 素材生成",
            ["Generative"] = "生成", // 10.1.x：assetgen-tab 文案由 "Asset Gen" 改为 "Generative"
            ["Deps"] = "依赖",
            ["Advanced"] = "高级",

            // Connection（Server）区块
            ["Server"] = "服务器",
            ["Transport:"] = "传输方式:",
            ["HTTP URL:"] = "HTTP 地址:",
            ["API Key:"] = "API 密钥:",
            ["Get API Key"] = "获取 API 密钥",
            ["Clear"] = "清除",
            ["Local Server:"] = "本地服务器:",
            ["Start Server"] = "启动服务器",
            ["Unity Socket Port:"] = "Unity 套接字端口:",
            ["Disconnected"] = "已断开",
            ["Connected"] = "已连接",
            ["Start"] = "启动",
            ["Stop"] = "停止",
            ["Manual Server Launch"] = "手动启动服务器",
            ["Use this command to launch the server manually:"] = "使用以下命令手动启动服务器:",
            ["Copy"] = "复制",

            // Advanced Settings 区块
            ["Advanced Settings"] = "高级设置",
            ["UVX Path:"] = "UVX 路径:",
            ["Browse"] = "浏览",
            ["Server Source:"] = "服务器来源:",
            ["Select"] = "选择",
            ["Debug Logging:"] = "调试日志:",
            ["Log Record (Assets/mcp.log):"] = "日志记录 (Assets/mcp.log):",
            ["Server Health:"] = "服务器健康状态:",
            ["Unknown"] = "未知",
            ["Test"] = "测试",
            ["Auto-Start Server on Editor Load:"] = "编辑器加载时自动启动服务器:",
            ["Force Fresh Install:"] = "强制全新安装:",
            ["Allow LAN Bind (HTTP Local):"] = "允许局域网绑定 (HTTP 本地):",
            ["Allow Insecure Remote HTTP:"] = "允许不安全的远程 HTTP:",
            ["Screenshots Folder:"] = "截图文件夹:",
            ["Package Source:"] = "包来源:",
            ["Deploy"] = "部署",
            ["Restore"] = "还原",

            // Script Validation 区块
            ["Script Validation"] = "脚本校验",
            ["Validation Level:"] = "校验级别:",
            // 校验级别下方的说明文字：源码里是多行 switch 表达式，各分支字符串各占一行（不在含 .text 的那一行上），
            // 补丁引擎需要靠 "跨行 switch 块" 识别逻辑才能命中这几条，见 McpForUnitySourcePatcher.ApplyTextReplacements。
            ["Basic: Validates syntax only. Fast compilation checks."] = "基础：仅校验语法，编译检查速度快。",
            ["Standard (Recommended): Checks syntax + common errors. Balanced speed and coverage."] = "标准（推荐）：校验语法及常见错误，速度与覆盖面均衡。",
            ["Comprehensive: Detailed validation including code quality. Slower but thorough."] = "全面：包含代码质量在内的详细校验，速度较慢但更彻底。",
            ["Strict: Maximum validation + warnings as errors. Slowest but catches all issues."] = "严格：最高级别校验，警告视为错误，速度最慢但能发现所有问题。",
            ["Unknown validation level"] = "未知校验级别",

            // Optional Dependencies 区块
            ["Optional Dependencies"] = "可选依赖",
            ["Some tool groups require optional packages. Install them to unlock additional capabilities."] = "部分工具组需要可选包才能使用，安装后可解锁更多功能。",
            ["Install All"] = "全部安装",
            ["Uninstall All"] = "全部卸载",
            ["Installing..."] = "安装中...",
            ["Removing..."] = "移除中...",
            ["Install"] = "安装",
            ["Uninstall"] = "卸载",
            ["Installed"] = "已安装",
            ["Not installed"] = "未安装",

            // Tools 区块
            ["Discovering tools..."] = "正在发现工具...",
            ["Project-Scoped Tools:"] = "项目范围工具:",
            ["Enable All"] = "全部启用",
            ["Disable All"] = "全部禁用",
            ["Rescan"] = "重新扫描",
            ["Reconfigure Clients"] = "重新配置客户端",
            ["Changes apply after reconnecting or re-registering tools."] = "更改需重新连接或重新注册工具后生效。",

            // Resources 区块
            ["Discovering resources..."] = "正在发现资源...",
            ["Changes apply after reconnecting or re-registering resources."] = "更改需重新连接或重新注册资源后生效。",

            // Client Configuration 区块
            ["Client Configuration"] = "客户端配置",
            ["Configure All Detected Clients"] = "配置所有检测到的客户端",
            ["Per-client setup"] = "按客户端单独设置",
            ["Client:"] = "客户端:",
            ["Not Configured"] = "未配置",
            ["Configure"] = "配置",
            ["Install Skills"] = "安装 Skills",
            ["Claude CLI Path:"] = "Claude CLI 路径:",
            ["Client Project Dir:"] = "客户端项目目录:",
            ["Manual Configuration"] = "手动配置",
            ["Config Path:"] = "配置文件路径:",
            ["Configuration:"] = "配置内容:",
            ["Open"] = "打开",
            ["Installation Steps:"] = "安装步骤:",

            // Asset Gen 区块
            ["AI Asset Generation"] = "AI 素材生成",
            ["Enter your own provider API keys. Generation is triggered via MCP tools / CLI, not here. Keys are stored in your OS secure store (Keychain / Credential Manager / libsecret), never in the project."] = "请输入你自己的服务商 API 密钥。素材生成通过 MCP 工具 / CLI 触发，而非在此处操作。密钥保存在操作系统的安全存储中（Keychain / 凭据管理器 / libsecret），不会存入项目。",
            ["GLB import needs the glTFast package — install it from the Dependencies tab."] = "导入 GLB 需要 glTFast 包 —— 请在依赖标签页中安装。",
            ["Preferences"] = "偏好设置",
            ["Default 3D Format:"] = "默认 3D 格式:",
            ["Output Root:"] = "输出根目录:",
            ["Auto-Normalize Imported Models:"] = "自动归一化导入的模型:",

            // Connection 区块动态状态文本
            ["Error"] = "错误",
            ["Transport Mismatch"] = "传输方式不匹配",
            ["No Session"] = "无会话",
            ["Resuming..."] = "恢复中...",
            ["Starting…"] = "启动中…",
            ["Run this command in your shell if you prefer to start the server manually."] = "如果你想手动启动服务器，请在终端运行此命令。",
            ["End Session"] = "结束会话",
            ["Disconnect"] = "断开连接",
            ["Start Session"] = "开始会话",
            ["HTTP Remote URL is blocked by current security settings."] = "HTTP 远程地址已被当前安全设置阻止。",
            ["HTTP Local URL is blocked by current security settings."] = "HTTP 本地地址已被当前安全设置阻止。",

            // Deps 区块动态状态文本
            ["Last backup: none"] = "上次备份：无",
            ["⚠ Missing dependencies. MCP for Unity requires all dependencies to function."] = "⚠ 缺少依赖项。MCP for Unity 需要所有依赖才能正常运行。",
            ["✓ All requirements met! MCP for Unity is ready to use."] = "✓ 所有要求已满足！MCP for Unity 已可以使用。",
            ["Not Found"] = "未找到",

            // Tools/Resources 区块动态状态文本
            ["No MCP resources discovered."] = "未发现任何 MCP 资源。",
            ["No MCP tools discovered."] = "未发现任何 MCP 工具。",

            // Client Configuration 区块动态状态文本
            ["Syncing..."] = "同步中...",
            ["Checking..."] = "检查中...",
            ["Configuration steps not available for this client."] = "此客户端暂无可用的配置步骤。",

            // Tooltip（悬浮提示）
            ["Allow HTTP Remote over plaintext http/ws. Disabled by default to require HTTPS/WSS."] = "允许 HTTP 远程使用明文 http/ws。默认关闭，要求使用 HTTPS/WSS。",
            ["Allow HTTP Local to bind on all interfaces (0.0.0.0 / ::). Disabled by default because devices on your LAN may reach MCP tools."] = "允许 HTTP 本地绑定到所有网络接口 (0.0.0.0 / ::)。默认关闭，因为局域网内的设备可能访问到 MCP 工具。",
            ["API key for remote-hosted MCP server authentication"] = "用于远程托管 MCP 服务器身份验证的 API 密钥",
            ["Uniformly scale imported models to the target size on import."] = "导入时将模型统一缩放到目标尺寸。",
            ["Automatically start the local HTTP server and connect the MCP bridge when the Unity Editor opens. Only applies to HTTP transport (stdio always auto-starts)."] = "Unity 编辑器打开时自动启动本地 HTTP 服务器并连接 MCP 桥接。仅对 HTTP 传输方式生效（stdio 始终自动启动）。",
            ["Select MCPForUnity source folder"] = "选择 MCPForUnity 源码文件夹",
            ["Select local server source folder"] = "选择本地服务器源码文件夹",
            ["Pick a folder inside the project; the path is stored project-relative."] = "在项目内选择一个文件夹；路径将以项目相对路径保存。",
            ["Browse for uvx executable"] = "浏览选择 uvx 可执行文件",
            ["Clear deployment source path"] = "清除部署源路径",
            ["Clear override and use default PyPI package"] = "清除覆盖设置，使用默认的 PyPI 包",
            ["Clear override and use the built-in default (Assets/Screenshots)."] = "清除覆盖设置，使用内置默认值 (Assets/Screenshots)。",
            ["Clear override and use auto-detection"] = "清除覆盖设置，使用自动检测",
            ["An API key is required for HTTP Remote. Enter one above."] = "HTTP 远程模式需要 API 密钥，请在上方输入。",
            ["Start or end the MCP session between Unity and the server."] = "开始或结束 Unity 与服务器之间的 MCP 会话。",
            ["Enable verbose debug logging to the Unity Console."] = "在 Unity 控制台启用详细调试日志。",
            ["Copy MCPForUnity to this project's package location"] = "将 MCPForUnity 复制到本项目的包位置",
            ["Restore the last backup before deployment"] = "还原部署前的最后一次备份",
            ["Copy a MCPForUnity folder into this project's package location."] = "将一个 MCPForUnity 文件夹复制到本项目的包位置。",
            ["When enabled, generated uvx commands add '--no-cache --refresh' before launching (slower startup, but avoids stale cached builds while iterating on the Server)."] = "启用后，生成的 uvx 命令会在启动前加上 '--no-cache --refresh'（启动变慢，但可避免在迭代 Server 时使用过期的缓存构建）。",
            ["Default container format for generated 3D models."] = "生成的 3D 模型默认的容器格式。",
            ["Capture a game camera screenshot. Default: Assets/Screenshots (configurable in Advanced)."] = "捕获游戏摄像机截图。默认路径：Assets/Screenshots（可在高级设置中配置）。",
            ["Override server source for uvx --from. Leave empty to use default PyPI package. Example local dev: /path/to/unity-mcp/Server"] = "覆盖 uvx --from 使用的服务器源。留空则使用默认 PyPI 包。本地开发示例：/path/to/unity-mcp/Server",
            ["HTTP endpoint URL for the MCP server. Use localhost for local servers."] = "MCP 服务器的 HTTP 端点地址。本地服务器请使用 localhost。",
            ["Log every MCP tool execution (tool, action, status, duration) to Assets/UnityMCP/Log/mcp.log."] = "将每次 MCP 工具执行（工具、操作、状态、耗时）记录到 Assets/UnityMCP/Log/mcp.log。",
            ["Capture a 6-angle contact sheet around the scene centre. Default: Assets/Screenshots (configurable in Advanced)."] = "围绕场景中心捕获 6 个角度的联系表截图。默认路径：Assets/Screenshots（可在高级设置中配置）。",
            ["When enabled, register project-scoped tools with HTTP Local and stdio transports. Allows per-project tool customization."] = "启用后，会以 HTTP 本地和 stdio 传输方式注册项目范围的工具，允许按项目自定义工具。",
            ["Capture the active Scene View viewport. Default: Assets/Screenshots (configurable in Advanced)."] = "捕获当前 Scene 视图。默认路径：Assets/Screenshots（可在高级设置中配置）。",
            ["Test the connection between Unity and the MCP server."] = "测试 Unity 与 MCP 服务器之间的连接。",
            ["Port for Unity's internal MCP bridge socket. Used for stdio transport."] = "Unity 内部 MCP 桥接套接字使用的端口，用于 stdio 传输方式。",
            ["Override path to uvx executable. Leave empty for auto-detection."] = "覆盖 uvx 可执行文件路径，留空则自动检测。",

            // MCP for Unity Setup 向导
            ["MCP for Unity Setup"] = "MCP for Unity 安装向导",
            ["System Requirements"] = "系统要求",
            ["MCP for Unity requires Python 3.10+ and UV package manager to function."] = "MCP for Unity 需要 Python 3.10+ 和 UV 包管理器才能运行。",
            ["Installation Instructions"] = "安装说明",
            ["Open Python Install Page"] = "打开 Python 安装页面",
            ["Open UV Install Page"] = "打开 UV 安装页面",
            ["Refresh"] = "刷新",
            ["Done"] = "完成",
            ["Configure MCP Clients"] = "配置 MCP 客户端",
            ["We found the following MCP clients on your machine. Select which to configure:"] = "在你的电脑上找到了以下 MCP 客户端，请选择要配置的对象:",
            ["Skip"] = "跳过",
            ["Configure Selected"] = "配置所选项",
            // 10.1.x：依赖状态改版新增
            ["Available"] = "可用",
            ["Not available"] = "不可用",
            ["Install UV Automatically"] = "自动安装 UV",
            ["Installing UV…"] = "正在安装 UV…",
            ["Installing uv… this can take a moment."] = "正在安装 uv…，这可能需要一点时间。",
            ["UV Package Manager"] = "UV 包管理器",
            ["Next"] = "下一步",

            // EditorPrefs 管理器
            ["EditorPrefs Manager"] = "EditorPrefs 管理器",
            ["Manage MCP for Unity EditorPrefs. Useful for development and testing."] = "管理 MCP for Unity 的 EditorPrefs，供开发和测试使用。",
            ["Create"] = "创建",
            ["Cancel"] = "取消",
            ["Save changes"] = "保存更改", // 10.1.x：EditorPrefItem 保存按钮 tooltip
            ["Refresh prefs"] = "刷新偏好设置", // 10.1.x：EditorPrefsWindow 刷新按钮 tooltip

            // 10.1.x：Connection 区块新增
            ["Stop Server"] = "停止服务器",
            ["The command is not available with the current configuration."] = "当前配置下该命令不可用。",

            // 10.1.x：Advanced / Asset Gen 区块新增（placeholder-text 与 tooltip）
            ["/path/to/Server or git+https://..."] = "/path/to/Server 或 git+https://...",
            ["Assets/Screenshots (default)"] = "Assets/Screenshots（默认）",
            ["The model generate_* uses for this provider when no explicit model is passed."] = "当未显式指定模型时，该服务商 generate_* 使用的模型。",
        };

        /// <summary>
        /// 带插值的动态字符串模板：直接匹配源码里保留插值表达式（<c>{...}</c>）不动、只翻译周围字面文字。
        /// 应用范围为整份文件内容的全文正则替换。
        /// </summary>
        public static readonly List<(Regex Pattern, string Replacement)> InterpolatedTemplates = new List<(Regex, string)>
        {
            (new Regex(@"\$""Update available: v\{(.+?)\}\s+\(current: v\{(.+?)\}\)"""),
                @"$""有可用更新: v{$1}（当前 v{$2}）"""),
            (new Regex(@"\$""Last backup: \{(.+?)\}"""),
                @"$""上次备份: {$1}"""),
            (new Regex(@"\$""Target: \{(.+?)\}"""),
                @"$""目标: {$1}"""),
            (new Regex(@"\$""\{(.+?)\} of \{(.+?)\} resources enabled\."""),
                @"$""已启用 {$1} / {$2} 个资源。"""),
            (new Regex(@"\$""\{(.+?)\} of \{(.+?)\} tools will register with connected clients\."""),
                @"$""将有 {$1} / {$2} 个工具注册到已连接的客户端。"""),
            (new Regex(@"\$""Session Active \(\{(.+?)\}\)"""),
                @"$""会话进行中 ({$1})"""),

            // 10.1.x：更新提示 tooltip（MCPForUnityEditorWindow，两行版本号 + 转义换行符）
            (new Regex(@"\$""Latest version: v\{(.+?)\}\\nCurrent version: v\{(.+?)\}"""),
                @"$""最新版本: v{$1}\n当前版本: v{$2}"""),

            // 10.1.x：客户端版本不匹配提示（UpdateVersionMismatchWarning，与传输方式不匹配提示是两条不同文本）
            (new Regex(@"\$""⚠ \{(.+?)\}: \{(.+?)\}"""),
                @"$""⚠ {$1}：{$2}"""),

            // 10.1.x：HTTP Local 需要回环地址提示（同一句在两处出现，其中一处嵌套在另一个插值字符串内部，
            // 靠"整份文件内容级正则替换"天然覆盖嵌套场景，无需特殊处理）
            (new Regex(@"\$""HTTP Local requires a loopback URL \(\{(.+?)\}\)\."""),
                @"$""HTTP 本地服务需要回环地址 URL（{$1}）。"""),

            // 10.1.x：Tools 区块 —— 分组标题的"已启用/总数"计数（同一形状出现在三处，变量名不同但literal "title" 相同）
            (new Regex(@"\$""\{title\} \(\{(.+?)\}/\{(.+?)\}\)"""),
                @"$""{title}（{$1}/{$2}）"""),
            // 10.1.x：Tools 区块 —— 分组勾选框 tooltip
            (new Regex(@"\$""Toggle all tools in \\""\{(.+?)\}\\"" on or off\."""),
                @"$""开启或关闭 \""{$1}\"" 中的所有工具。"""),
            // 10.1.x：Tools 区块 —— batch_execute 上限设置 tooltip
            (new Regex(@"\$""Number of commands allowed per batch_execute call \(1–\{(.+?)\}\)\. Default: \{(.+?)\}\."""),
                @"$""每次 batch_execute 调用允许的命令数量（1–{$1}）。默认值：{$2}。"""),

            // 10.1.x：Asset Gen 区块 —— 启用某个素材生成服务商的 tooltip
            (new Regex(@"\$""Enable the \{(.+?)\} provider for asset generation\."""),
                @"$""启用 {$1} 素材生成服务商。"""),

            // 客户端配置 - 传输方式不匹配提示（连接页 transportMismatchText，插值 + 转义引号，跨行拼接）
            // 保留传输方式技术名（stdio / HTTP Local / HTTP Remote）不翻译，只译周围文字。
            (new Regex(@"\$""⚠ \{(.+?)\} is configured for \\""\{(.+?)\}\\"" but server is set to \\""\{(.+?)\}\\""\. """),
                @"$""⚠ {$1} 当前配置为 \""{$2}\""，但服务器设为 \""{$3}\""。 """),
            (new Regex(@"""Click \\""Configure\\"" in Client Configuration to update\."""),
                @"""点击客户端配置中的 \""配置\"" 进行更新。"""),

            // 客户端配置 - 状态 tag（GetStatusDisplayString switch，带枚举成员前缀精确匹配，避免误伤逻辑 switch）
            (new Regex(@"McpStatus\.NotConfigured => ""Not Configured"""), @"McpStatus.NotConfigured => ""未配置"""),
            (new Regex(@"McpStatus\.Configured => ""Configured"""), @"McpStatus.Configured => ""已配置"""),
            (new Regex(@"McpStatus\.Running => ""Running"""), @"McpStatus.Running => ""运行中"""),
            (new Regex(@"McpStatus\.Connected => ""Connected"""), @"McpStatus.Connected => ""已连接"""),
            (new Regex(@"McpStatus\.IncorrectPath => ""Incorrect Path"""), @"McpStatus.IncorrectPath => ""路径错误"""),
            (new Regex(@"McpStatus\.CommunicationError => ""Communication Error"""), @"McpStatus.CommunicationError => ""通信错误"""),
            (new Regex(@"McpStatus\.NoResponse => ""No Response"""), @"McpStatus.NoResponse => ""无响应"""),
            (new Regex(@"McpStatus\.UnsupportedOS => ""Unsupported OS"""), @"McpStatus.UnsupportedOS => ""不支持的操作系统"""),
            (new Regex(@"McpStatus\.MissingConfig => ""Missing MCPForUnity Config"""), @"McpStatus.MissingConfig => ""缺少 MCPForUnity 配置"""),
            (new Regex(@"McpStatus\.Error => ""Error"""), @"McpStatus.Error => ""错误"""),
            (new Regex(@"McpStatus\.VersionMismatch => ""Version Mismatch"""), @"McpStatus.VersionMismatch => ""版本不匹配"""),
            (new Regex(@"_ => ""Unknown"","), @"_ => ""未知"","),

            // 客户端配置 - Configure/Unregister 进行中的临时状态（三元表达式，不在 .text 行上）
            (new Regex(@"\? ""Unregistering\.\.\."" : ""Configuring\.\.\."""),
                @"? ""取消注册中..."" : ""注册中..."""),
        };
    }
}
#endif
