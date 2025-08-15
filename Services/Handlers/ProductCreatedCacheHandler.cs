using Core.Dtos.Mediator;
using Mediator;
using Microsoft.Extensions.Logging;

namespace Services.Handlers;

/// <summary>
/// 产品创建缓存更新处理器
/// </summary>
public class ProductCreatedCacheHandler : INotificationHandler<ProductCreatedNotification>
{
    private readonly ILogger<ProductCreatedCacheHandler> _logger;

    public ProductCreatedCacheHandler(ILogger<ProductCreatedCacheHandler> logger)
    {
        _logger = logger;
    }

    public ValueTask Handle(ProductCreatedNotification notification, CancellationToken cancellationToken)
    {
        // 模拟缓存更新操作
        _logger.LogInformation(
            "[缓存处理器] 已更新产品缓存 - 产品: {Name} (ID: {ProductId}) 已添加到缓存中",
            notification.Name,
            notification.ProductId);

        return ValueTask.CompletedTask;
    }
}