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

    /// <summary>
    /// 获取所有订单
    /// </summary>
    /// <returns></returns>
    Task<List<OrderEntity>> GetAllAsync();

    /// <summary>
    /// 创建订单实体
    /// </summary>
    /// <param name="order"></param>
    /// <returns></returns>
    Task<OrderEntity> CreateAsync(OrderEntity order);

    /// <summary>
    /// 更新订单
    /// </summary>
    /// <param name="order"></param>
    /// <returns></returns>
    Task<OrderEntity> UpdateAsync(OrderEntity order);

    /// <summary>
    /// 根据订单类型获取订单
    /// </summary>
    /// <param name="orderType"></param>
    /// <returns></returns>
    Task<List<OrderEntity>> GetByTypeAsync(OrderType orderType);

    /// <summary>
    /// 根据电话号码获取订单
    /// </summary>
    /// <param name="phone"></param>
    /// <returns></returns>
    Task<List<OrderEntity>> GetByPhoneAsync(string phone);

    /// <summary>
    /// 删除订单（返回是否成功）
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<bool> DeleteByIdAsync(Guid id);
}