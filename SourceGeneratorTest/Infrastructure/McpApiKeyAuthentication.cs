using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.Extensions.Options;

namespace SourceGeneratorTest.Infrastructure;

/// <summary>
/// MCP API Key 认证方案的默认值
/// </summary>
public static class McpApiKeyDefaults
{
    public const string Scheme = "McpApiKey";
    public const string DisplayName = "MCP API Key";
}

/// <summary>
/// MCP API Key 配置，对应 appsettings 中的 McpAuth 节点
/// </summary>
public sealed class McpApiKeyOptions
{
    public const string SectionName = "McpAuth";

    public string HeaderName { get; set; } = "X-API-Key";

    public McpApiKeyEntry[] Keys { get; set; } = [];
}

public sealed class McpApiKeyEntry
{
    /// <summary>
    /// 调用方名称，用于写入 Claims 便于审计
    /// </summary>
    public string Name { get; set; } = string.Empty;

    public string Key { get; set; } = string.Empty;

    public string[] Roles { get; set; } = [];
}

/// <summary>
/// MCP 工具级鉴权使用的角色
/// </summary>
public static class McpRoles
{
    public const string Read = "mcp.read";
    public const string Write = "mcp.write";
}

/// <summary>
/// MCP 工具级鉴权使用的策略名
/// </summary>
public static class McpPolicies
{
    public const string Write = "McpWrite";
}

/// <summary>
/// 通过请求头校验 API Key 的认证处理器，校验通过后按配置写入角色声明
/// </summary>
public sealed class McpApiKeyAuthenticationHandler(
    IOptionsMonitor<AuthenticationSchemeOptions> options,
    ILoggerFactory logger,
    UrlEncoder encoder,
    IOptions<McpApiKeyOptions> apiKeyOptions)
    : AuthenticationHandler<AuthenticationSchemeOptions>(options, logger, encoder)
{
    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var settings = apiKeyOptions.Value;
        if (!Request.Headers.TryGetValue(settings.HeaderName, out var headerValues) || headerValues.Count == 0)
            return Task.FromResult(AuthenticateResult.NoResult());

        var providedKey = headerValues.ToString();
        if (string.IsNullOrWhiteSpace(providedKey))
            return Task.FromResult(AuthenticateResult.Fail($"请求头 {settings.HeaderName} 为空"));

        var providedKeyBytes = Encoding.UTF8.GetBytes(providedKey);

        foreach (var entry in settings.Keys)
        {
            if (entry.Key.Length == 0)
                continue;

            // 定长比较，避免通过响应时间推断密钥
            if (!CryptographicOperations.FixedTimeEquals(providedKeyBytes, Encoding.UTF8.GetBytes(entry.Key)))
                continue;

            var claims = new List<Claim>
            {
                new(ClaimTypes.Name, entry.Name),
                new("api_key_name", entry.Name)
            };
            claims.AddRange(entry.Roles.Select(role => new Claim(ClaimTypes.Role, role)));

            var identity = new ClaimsIdentity(claims, Scheme.Name);
            return Task.FromResult(AuthenticateResult.Success(
                new AuthenticationTicket(new ClaimsPrincipal(identity), Scheme.Name)));
        }

        return Task.FromResult(AuthenticateResult.Fail("API Key 无效"));
    }
}

/// <summary>
/// 避免 HttpLogging 把 MCP 请求头（含 API Key）写入控制台与日志文件
/// </summary>
public sealed class McpSensitiveHeaderLoggingInterceptor : IHttpLoggingInterceptor
{
    public ValueTask OnRequestAsync(HttpLoggingInterceptorContext logContext)
    {
        if (logContext.HttpContext.Request.Path.StartsWithSegments("/mcp"))
            logContext.LoggingFields &= ~HttpLoggingFields.RequestHeaders;

        return default;
    }

    public ValueTask OnResponseAsync(HttpLoggingInterceptorContext logContext) => default;
}
