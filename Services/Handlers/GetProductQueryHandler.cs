using Mediator;
using Core.Dtos;
using Core.Dtos.Mediator;
using Core.Dtos.Mappers;
using Microsoft.Extensions.Logging;
using Repositories;

namespace Services.Handlers;

/// <summary>
/// 获取产品查询处理器
/// </summary>
public class GetProductQueryHandler : IRequestHandler<GetProductQuery, ProductDto>
{
    private readonly IProductRepository _productRepository;
    private readonly ILogger<GetProductQueryHandler> _logger;

    public GetProductQueryHandler(IProductRepository productRepository, ILogger<GetProductQueryHandler> logger)
    {
        _productRepository = productRepository;
        _logger = logger;
    }

    public async ValueTask<ProductDto> Handle(GetProductQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling GetProductQuery for ProductId: {ProductId}", request.ProductId);
        
        var product = await _productRepository.GetByIdAsync(request.ProductId);
        if (product == null)
        {
            _logger.LogWarning("Product with ID {ProductId} not found", request.ProductId);
            throw new ArgumentNullException(nameof(product), "产品不存在");
        }

        var dto = new ProductMapper().EntityToDto(product);
        _logger.LogInformation("Successfully retrieved product: {@ProductDto}", dto);
        
        return dto;
    }
}