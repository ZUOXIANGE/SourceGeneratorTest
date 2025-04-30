using Core.Entities;

namespace Repositories;

public interface IOrderRepository
{
    /// <summary>
    /// 添加
    /// </summary>
    /// <param name="order"></param>
    /// <returns></returns>
    Task<Guid> AddAsync(OrderEntity order);

    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task DeleteAsync(Guid id);

    /// <summary>
    /// 查询
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<OrderEntity?> GetByIdAsync(Guid id);
}