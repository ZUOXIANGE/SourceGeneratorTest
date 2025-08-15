using Core.Dtos.Mediator;
using Mediator;
using Microsoft.Extensions.Logging;

namespace Services.Handlers;

/// <summary>
/// 产品创建邮件通知处理器
/// </summary>
public class ProductCreatedEmailHandler : INotificationHandler<ProductCreatedNotification>
{
    private readonly ILogger<ProductCreatedEmailHandler> _logger;

    public ProductCreatedEmailHandler(ILogger<ProductCreatedEmailHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask Handle(ProductCreatedNotification notification, CancellationToken cancellationToken)
    {
        // 模拟发送邮件的异步操作
        await Task.Delay(100, cancellationToken);
        
        _logger.LogInformation(
            "[邮件处理器] 已发送产品创建通知邮件 - 产品: {Name} (ID: {ProductId}), 价格: {Price}",
            notification.Name,
            notification.ProductId,
            notification.Price);
    }
}