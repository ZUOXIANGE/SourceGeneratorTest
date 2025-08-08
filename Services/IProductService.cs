using Core.Dtos;

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
}