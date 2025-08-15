using Mediator;

namespace Core.Dtos.Mediator;

/// <summary>
/// 产品创建成功通知
/// </summary>
public class ProductCreatedNotification : INotification
{
    public Guid ProductId { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public DateTime CreatedAt { get; set; }
    
    public ProductCreatedNotification(Guid productId, string name, decimal price)
    {
        ProductId = productId;
        Name = name;
        Price = price;
        CreatedAt = DateTime.UtcNow;
    }
}