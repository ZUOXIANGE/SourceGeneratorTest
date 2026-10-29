using System.Text.Json;
using ModelContextProtocol.Client;
using ModelContextProtocol.Protocol;

// MCP 服务地址：与服务端 app.MapMcp("/mcp") 对应，可通过第一个命令行参数覆盖
var endpoint = args.Length > 0 ? args[0] : "http://localhost:5088/mcp";

// 服务端 McpAuth 中配置的 API Key：第二个参数 > 环境变量 MCP_API_KEY > 默认的写权限 Key
var apiKey = args.Length > 1
    ? args[1]
    : Environment.GetEnvironmentVariable("MCP_API_KEY") ?? "dev-mcp-write-key";

// Streamable HTTP 传输（服务端为 Stateless 模式，无需保持会话）
var transport = new HttpClientTransport(new HttpClientTransportOptions
{
    Endpoint = new Uri(endpoint),
    TransportMode = HttpTransportMode.StreamableHttp,
    ConnectionTimeout = TimeSpan.FromSeconds(30),
    AdditionalHeaders = new Dictionary<string, string>
    {
        ["X-API-Key"] = apiKey
    }
});

await using var client = await McpClient.CreateAsync(transport);

Console.WriteLine($"已连接 MCP 服务：{client.ServerInfo?.Name} {client.ServerInfo?.Version}");
Console.WriteLine($"会话 ID：{client.SessionId ?? "(无，Stateless)"}");

// 1. 列出服务端暴露的工具
Console.WriteLine("\n可用工具：");
foreach (var tool in await client.ListToolsAsync())
    Console.WriteLine($"- {tool.Name}：{tool.Description}");

// 2. 创建产品（参数名与工具 JSON Schema 一致）
var createResult = await client.CallToolAsync("create_product", new Dictionary<string, object?>
{
    ["name"] = "MCP 客户端示例产品",
    ["price"] = 199.9m,
    ["category"] = "Electronics",
    ["description"] = "由 McpClientSample 创建"
});
var productId = JsonSerializer.Deserialize<Guid>(GetText(createResult));
Console.WriteLine($"\ncreate_product -> {productId}");

// 3. 查询全部产品
var listResult = await client.CallToolAsync("list_products", new Dictionary<string, object?>());
Console.WriteLine($"list_products -> {GetText(listResult)}");

// 4. 按 ID 查询单个产品
var getResult = await client.CallToolAsync("get_product", new Dictionary<string, object?>
{
    ["productId"] = productId
});
Console.WriteLine($"get_product -> {GetText(getResult)}");

// 5. 删除产品（服务端为软删除）
var deleteResult = await client.CallToolAsync("delete_product", new Dictionary<string, object?>
{
    ["productId"] = productId
});
Console.WriteLine($"delete_product -> {GetText(deleteResult)}");

// 6. 业务错误：不存在的 ID，工具以 isError 返回而不是抛协议异常
var missingResult = await client.CallToolAsync("get_product", new Dictionary<string, object?>
{
    ["productId"] = Guid.Empty
});
Console.WriteLine($"get_product(不存在) -> IsError={missingResult.IsError}，{GetText(missingResult)}");

// 工具结果以文本块返回，这里统一取出拼接
static string GetText(CallToolResult result)
    => string.Join("\n", result.Content.OfType<TextContentBlock>().Select(block => block.Text));
