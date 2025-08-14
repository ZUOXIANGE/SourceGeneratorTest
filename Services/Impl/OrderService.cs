using AutoCtor;
using Core.Dtos;
using Core.Dtos.Mappers;
using Core.Entities;
using Microsoft.Extensions.Logging;
using Repositories;

namespace Services.Impl;

[AutoConstruct]
[RegisterScoped]
public partial class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly ILogger<OrderService> _logger;

    /// <inheritdoc />
    public async Task<Guid> CreateOrderAsync(CreateOrderReq req)
    {
        //new mapper 形式
        var order = new OrderMapper().ReqToEntity(req);
        //静态扩展方法形式
        var order2 = req.ToEntity();
        _logger.LogInformation("Order:{@order}", order2);
        return await _orderRepository.AddAsync(order);
    }

    /// <summary>
    /// 获取订单
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<OrderDto> GetOrderAsync(Guid id)
    {
        var order = await _orderRepository.GetByIdAsync(id);
        if (order == null)
            throw new ArgumentNullException(nameof(order));

        var orderClone = new CloneGenerator().Clone(order);
        var clone2 = order.Clone();
        _logger.LogInformation("OrderClone:{@order}", clone2);

        var type = orderClone.OrderType.ToStringFast(true);
        var dto = new OrderMapper().EntityToDto(order);
        if (dto.OrderTypeDesc != type)
            throw new Exception("订单类型映射错误");
        return dto;
    }

    public async Task<List<OrderEntity>> GetAllOrdersAsync()
    {
        return await _orderRepository.GetAllAsync();
    }

    public async Task<OrderEntity?> GetOrderByIdAsync(Guid id)
    {
        return await _orderRepository.GetByIdAsync(id);
    }

    public async Task<OrderEntity> CreateOrderEntityAsync(OrderEntity order)
    {
        return await _orderRepository.CreateAsync(order);
    }

    public async Task<OrderEntity?> UpdateOrderAsync(Guid id, OrderEntity order)
    {
        var existingOrder = await _orderRepository.GetByIdAsync(id);
        
        if (existingOrder == null)
            return null;

        existingOrder.Name = order.Name;
        existingOrder.Phone = order.Phone;
        existingOrder.Address = order.Address;
        existingOrder.OrderType = order.OrderType;

        return await _orderRepository.UpdateAsync(existingOrder);
    }

    public async Task<bool> DeleteOrderAsync(Guid id)
    {
        return await _orderRepository.DeleteByIdAsync(id);
    }

    public async Task<List<OrderEntity>> GetOrdersByTypeAsync(OrderType orderType)
    {
        return await _orderRepository.GetByTypeAsync(orderType);
    }

    public async Task<List<OrderEntity>> GetOrdersByPhoneAsync(string phone)
    {
        return await _orderRepository.GetByPhoneAsync(phone);
    }
}