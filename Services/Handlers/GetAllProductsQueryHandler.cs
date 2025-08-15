using Mediator;
using Core.Dtos;
using Core.Dtos.Mediator;
using Core.Dtos.Mappers;
using Microsoft.Extensions.Logging;
using Repositories;

namespace Services.Handlers;

/// <summary>
/// 获取所有产品查询处理器
/// </summary>
public class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQuery, List<ProductDto>>
{
    private readonly IProductRepository _productRepository;
    private readonly ILogger<GetAllProductsQueryHandler> _logger;

    public GetAllProductsQueryHandler(IProductRepository productRepository, ILogger<GetAllProductsQueryHandler> logger)
    {
        _productRepository = productRepository;
        _logger = logger;
    }

    public async ValueTask<List<ProductDto>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling GetAllProductsQuery");
        
        var products = await _productRepository.GetAllAsync();
        var mapper = new ProductMapper();
        var dtos = products.Select(p => mapper.EntityToDto(p)).ToList();
        
        _logger.LogInformation("Successfully retrieved {Count} products", dtos.Count);
        
        return dtos;
    }
}