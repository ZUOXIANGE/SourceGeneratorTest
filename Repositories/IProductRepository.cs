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
}