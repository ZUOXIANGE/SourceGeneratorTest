using AutoCtor;
using Core.Entities;
using Microsoft.Extensions.Caching.Memory;

namespace Repositories.Impl;

[AutoConstruct]
public partial class ProductRepository : IProductRepository
{
    private readonly IMemoryCache _memoryCache;
    private const string PRODUCTS_KEY = "all_products";

    /// <summary>
    /// 添加产品
    /// </summary>
    /// <param name="product"></param>
    /// <returns></returns>
    public async Task<Guid> AddAsync(ProductEntity product)
    {
        await Task.Delay(1);
        var id = Guid.NewGuid();
        product.ProductId = id;
        
        var products = GetProductsFromCache();
        products.Add(product);
        _memoryCache.Set(PRODUCTS_KEY, products);
        
        return id;
    }

    /// <summary>
    /// 根据ID获取产品
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<ProductEntity?> GetByIdAsync(Guid id)
    {
        await Task.Delay(1);
        var products = GetProductsFromCache();
        return products.FirstOrDefault(p => p.ProductId == id);
    }

    /// <summary>
    /// 删除产品
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task DeleteAsync(Guid id)
    {
        await Task.Delay(1);
        var products = GetProductsFromCache();
        var product = products.FirstOrDefault(p => p.ProductId == id);
        if (product != null)
        {
            products.Remove(product);
            _memoryCache.Set(PRODUCTS_KEY, products);
        }
    }

    /// <summary>
    /// 获取所有产品
    /// </summary>
    /// <returns></returns>
    public async Task<List<ProductEntity>> GetAllAsync()
    {
        await Task.Delay(1);
        return GetProductsFromCache();
    }

    private List<ProductEntity> GetProductsFromCache()
    {
        return _memoryCache.GetOrCreate(PRODUCTS_KEY, _ => new List<ProductEntity>()) ?? new List<ProductEntity>();
    }
}