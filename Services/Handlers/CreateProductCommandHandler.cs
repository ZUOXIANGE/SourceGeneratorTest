using Mediator;
using Core.Dtos.Mediator;
using Core.Dtos.Mappers;
using Core.Entities;
using Microsoft.Extensions.Logging;
using Repositories;

namespace Services.Handlers;

/// <summary>
/// 创建产品命令处理器
/// </summary>
public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Guid>
{
    private readonly IProductRepository _productRepository;
    private readonly ILogger<CreateProductCommandHandler> _logger;
    private readonly IMediator _mediator;

    public CreateProductCommandHandler(IProductRepository productRepository, ILogger<CreateProductCommandHandler> logger, IMediator mediator)
    {
        _productRepository = productRepository;
        _logger = logger;
        _mediator = mediator;
    }

    public async ValueTask<Guid> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling CreateProductCommand for product: {ProductName}", request.Name);
        
        // 创建产品实体
        var product = new ProductEntity
        {
            ProductId = Guid.NewGuid(),
            Name = request.Name,
            Description = request.Description,
            Price = request.Price,
            Category = request.Category,
            CreatedAt = DateTime.UtcNow,
            IsAvailable = true
        };
        
        _logger.LogInformation("Creating product: {@Product}", product);
        
        var productId = await _productRepository.AddAsync(product);
        
        _logger.LogInformation("Successfully created product with ID: {ProductId}", productId);
        
        // 发布产品创建成功通知（广播）
        var notification = new ProductCreatedNotification(productId, product.Name, product.Price);
        await _mediator.Publish(notification, cancellationToken);
        
        return productId;
    }
}