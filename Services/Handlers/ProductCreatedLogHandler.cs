using Core.Dtos.Mediator;
using Mediator;
using Microsoft.Extensions.Logging;

namespace Services.Handlers;

/// <summary>
/// 产品创建日志处理器
/// </summary>
public class ProductCreatedLogHandler : INotificationHandler<ProductCreatedNotification>
{
    private readonly ILogger<ProductCreatedLogHandler> _logger;

    public ProductCreatedLogHandler(ILogger<ProductCreatedLogHandler> logger)
    {
        _logger = logger;
    }

    public ValueTask Handle(ProductCreatedNotification notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "[日志处理器] 产品创建成功 - ID: {ProductId}, 名称: {Name}, 价格: {Price}, 创建时间: {CreatedAt}",
            notification.ProductId,
            notification.Name,
            notification.Price,
            notification.CreatedAt);

        return ValueTask.CompletedTask;
    }
}