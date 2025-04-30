using Core.Dtos;

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
}