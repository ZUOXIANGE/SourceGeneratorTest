using Core.Dtos;
using Core.Options;
using Microsoft.Extensions.Options;
using Services;
using SourceGeneratorTest.Infrastructure;

namespace SourceGeneratorTest.Endpoints;

/// <summary>
/// 测试端点
/// </summary>
public class TestEndpoints : IEndpoint
{
    public static void MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/api/test").WithTags("测试").WithApiResult();

        group.MapGet("/ping", Ping)
            .WithSummary("Ping 测试接口")
            .WithDescription("用于测试服务可用性，返回字符串 'pong'，并输出当前配置。");

        group.MapPost("/createOrder", CreateOrderAsync)
            .WithSummary("创建订单")
            .WithDescription("创建一个新订单，返回生成的订单 ID。");

        group.MapGet("/getOrder", GetOrderAsync)
            .WithSummary("获取订单")
            .WithDescription("根据订单 ID 获取订单详情。");
    }

    private static string Ping(
        ILogger<TestEndpoints> logger,
        IOptionsMonitor<TestOptions> testOptionsMonitor,
        IOptionsMonitor<OrderOptions> orderOptionsMonitor,
        IOptions<AppSettings> appSettings)
    {
        logger.LogInformation("当前配置1:{@options}", testOptionsMonitor.CurrentValue);
        logger.LogInformation("当前配置2:{@options}", orderOptionsMonitor.CurrentValue);
        logger.LogInformation("当前配置3:{@options}", appSettings.Value);

        return "pong";
    }

    private static Task<Guid> CreateOrderAsync(CreateOrderReq req, IOrderService orderService)
        => orderService.CreateOrderAsync(req);

    private static async Task<OrderDto> GetOrderAsync(Guid id, IOrderService orderService)
    {
        try
        {
            return await orderService.GetOrderAsync(id);
        }
        catch (ArgumentNullException)
        {
            throw new ApiException(StatusCodes.Status404NotFound, "订单不存在");
        }
    }
}
