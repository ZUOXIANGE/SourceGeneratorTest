using Core.Dtos;
using Core.Dtos.Mediator;
using Mediator;
using SourceGeneratorTest.Infrastructure;

namespace SourceGeneratorTest.Endpoints;

/// <summary>
/// Mediator 演示端点
/// </summary>
public class MediatorEndpoints : IEndpoint
{
    public static void MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/api/mediator").WithTags("Mediator 演示").WithApiResult();

        group.MapPost("/products", CreateProductAsync)
            .WithSummary("通过 Mediator 创建产品")
            .WithDescription("发送创建产品命令，返回产品 ID。");

        group.MapGet("/products/{id:guid}", GetProductAsync)
            .WithSummary("通过 Mediator 获取产品")
            .WithDescription("发送获取产品查询，返回产品信息。");

        group.MapGet("/products", GetAllProductsAsync)
            .WithSummary("通过 Mediator 获取所有产品")
            .WithDescription("发送获取所有产品查询，返回产品列表。");

        group.MapGet("/health", HealthCheck)
            .WithSummary("Mediator 健康检查")
            .WithDescription("检查 Mediator 集成是否正常工作，返回健康状态。");

        group.MapPost("/broadcast-notification", BroadcastNotification)
            .WithSummary("发送广播通知示例")
            .WithDescription("发布产品创建通知，所有通知处理器都会被调用。");
    }

    private static async Task<Guid> CreateProductAsync(
        CreateProductCommand command,
        IMediator mediator,
        ILogger<MediatorEndpoints> logger)
    {
        logger.LogInformation("Creating product via Mediator: {ProductName}", command.Name);
        return await mediator.Send(command);
    }

    private static async Task<ProductDto> GetProductAsync(Guid id, IMediator mediator)
    {
        try
        {
            return await mediator.Send(new GetProductQuery(id));
        }
        catch (ArgumentNullException)
        {
            throw new ApiException(StatusCodes.Status404NotFound, "产品不存在");
        }
    }

    private static async Task<List<ProductDto>> GetAllProductsAsync(IMediator mediator)
        => await mediator.Send(new GetAllProductsQuery());

    private static MediatorHealthResponse HealthCheck()
        => new("Healthy", "Mediator integration is working properly", DateTime.UtcNow);

    private static async Task<BroadcastNotificationResponse> BroadcastNotification(
        BroadcastRequest request,
        IMediator mediator)
    {
        var notification = new ProductCreatedNotification(request.ProductId, request.Name, request.Price);
        await mediator.Publish(notification);

        return new BroadcastNotificationResponse("广播通知已发送", DateTime.UtcNow);
    }
}

/// <summary>
/// 广播请求模型
/// </summary>
public class BroadcastRequest
{
    public Guid ProductId { get; set; }

    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
}

/// <summary>
/// Mediator 健康检查响应
/// </summary>
public record MediatorHealthResponse(string Status, string Message, DateTime Timestamp);

/// <summary>
/// 广播通知响应
/// </summary>
public record BroadcastNotificationResponse(string Message, DateTime Timestamp);
