using Core.Dtos;
using Core.Entities;

namespace Services;

public interface IOrderService
{
    /// <summary>
    /// 创建订单
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    Task<Guid> CreateOrderAsync(CreateOrderReq req);

    /// <summary>
    /// 获取订单
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<OrderDto> GetOrderAsync(Guid id);

    /// <summary>
    /// 获取所有订单
    /// </summary>
    /// <returns></returns>
    Task<List<OrderEntity>> GetAllOrdersAsync();

    /// <summary>
    /// 根据ID获取订单
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<OrderEntity?> GetOrderByIdAsync(Guid id);

    /// <summary>
    /// 创建订单实体
    /// </summary>
    /// <param name="order"></param>
    /// <returns></returns>
    Task<OrderEntity> CreateOrderEntityAsync(OrderEntity order);

    /// <summary>
    /// 更新订单
    /// </summary>
    /// <param name="id"></param>
    /// <param name="order"></param>
    /// <returns></returns>
    Task<OrderEntity?> UpdateOrderAsync(Guid id, OrderEntity order);

    /// <summary>
    /// 删除订单
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<bool> DeleteOrderAsync(Guid id);

    /// <summary>
    /// 根据订单类型获取订单
    /// </summary>
    /// <param name="orderType"></param>
    /// <returns></returns>
    Task<List<OrderEntity>> GetOrdersByTypeAsync(OrderType orderType);

    /// <summary>
    /// 根据电话号码获取订单
    /// </summary>
    /// <param name="phone"></param>
    /// <returns></returns>
    Task<List<OrderEntity>> GetOrdersByPhoneAsync(string phone);
}