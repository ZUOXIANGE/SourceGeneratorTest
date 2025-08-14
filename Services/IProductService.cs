using Core.Dtos;
using Core.Entities;

namespace Services;

public interface IProductService
{
    /// <summary>
    /// 创建产品
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    Task<Guid> CreateProductAsync(CreateProductReq req);

    /// <summary>
    /// 获取产品
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<ProductDto> GetProductAsync(Guid id);

    /// <summary>
    /// 获取所有产品
    /// </summary>
    /// <returns></returns>
    Task<List<ProductDto>> GetAllProductsAsync();

    /// <summary>
    /// 删除产品
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task DeleteProductAsync(Guid id);

    /// <summary>
    /// 获取所有产品实体
    /// </summary>
    /// <returns></returns>
    Task<List<ProductEntity>> GetAllProductEntitiesAsync();

    /// <summary>
    /// 根据ID获取产品实体
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<ProductEntity?> GetProductByIdAsync(Guid id);

    /// <summary>
    /// 创建产品实体
    /// </summary>
    /// <param name="product"></param>
    /// <returns></returns>
    Task<ProductEntity> CreateProductEntityAsync(ProductEntity product);

    /// <summary>
    /// 更新产品
    /// </summary>
    /// <param name="id"></param>
    /// <param name="product"></param>
    /// <returns></returns>
    Task<ProductEntity?> UpdateProductAsync(Guid id, ProductEntity product);

    /// <summary>
    /// 删除产品实体
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<bool> DeleteProductEntityAsync(Guid id);

    /// <summary>
    /// 根据分类获取产品
    /// </summary>
    /// <param name="category"></param>
    /// <returns></returns>
    Task<List<ProductEntity>> GetProductsByCategoryAsync(ProductCategory category);
}