using AutoCtor;
using Core.Dtos;
using Core.Dtos.Mappers;
using Core.Entities;
using Microsoft.Extensions.Logging;
using Repositories;

namespace Services.Impl;

[AutoConstruct]
[RegisterScoped]
public partial class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;
    private readonly ILogger<ProductService> _logger;

    /// <inheritdoc />
    public async Task<Guid> CreateProductAsync(CreateProductReq req)
    {
        var product = new ProductMapper().ReqToEntity(req);
        // 也可以使用静态扩展方法
        var product2 = req.ToEntity();
        
        _logger.LogInformation("Creating product: {@product}", product);
        return await _productRepository.AddAsync(product);
    }

    /// <inheritdoc />
    public async Task<ProductDto> GetProductAsync(Guid id)
    {
        var product = await _productRepository.GetByIdAsync(id);
        if (product == null)
            throw new ArgumentNullException(nameof(product), "产品不存在");

        var dto = new ProductMapper().EntityToDto(product);
        _logger.LogInformation("Retrieved product: {@dto}", dto);
        return dto;
    }

    /// <inheritdoc />
    public async Task<List<ProductDto>> GetAllProductsAsync()
    {
        var products = await _productRepository.GetAllAsync();
        var mapper = new ProductMapper();
        var dtos = products.Select(p => mapper.EntityToDto(p)).ToList();
        
        _logger.LogInformation("Retrieved {count} products", dtos.Count);
        return dtos;
    }

    /// <inheritdoc />
    public async Task DeleteProductAsync(Guid id)
    {
        await _productRepository.DeleteAsync(id);
        _logger.LogInformation("Deleted product with id: {id}", id);
    }
}