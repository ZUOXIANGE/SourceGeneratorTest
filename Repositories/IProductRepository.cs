using Core.Entities;

namespace Repositories;

public interface IProductRepository
{
    /// <summary>
    /// 添加产品
    /// </summary>
    /// <param name="product"></param>
    /// <returns></returns>
    Task<Guid> AddAsync(ProductEntity product);

    /// <summary>
    /// 根据ID获取产品
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<ProductEntity?> GetByIdAsync(Guid id);

    /// <summary>
    /// 删除产品
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task DeleteAsync(Guid id);

    /// <summary>
    /// 获取所有产品
    /// </summary>
    /// <returns></returns>
    Task<List<ProductEntity>> GetAllAsync();

    /// <summary>
    /// 创建产品实体
    /// </summary>
    /// <param name="product"></param>
    /// <returns></returns>
    Task<ProductEntity> CreateAsync(ProductEntity product);

    /// <summary>
    /// 更新产品
    /// </summary>
    /// <param name="product"></param>
    /// <returns></returns>
    Task<ProductEntity> UpdateAsync(ProductEntity product);

    /// <summary>
    /// 根据分类获取产品
    /// </summary>
    /// <param name="category"></param>
    /// <returns></returns>
    Task<List<ProductEntity>> GetByCategoryAsync(ProductCategory category);

    /// <summary>
    /// 删除产品（返回是否成功）
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<bool> DeleteByIdAsync(Guid id);
}