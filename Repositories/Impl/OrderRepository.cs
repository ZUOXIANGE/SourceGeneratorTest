using AutoCtor;
using Core.Entities;
using Microsoft.Extensions.Caching.Memory;

namespace Repositories.Impl;

[AutoConstruct]
public partial class OrderRepository : IOrderRepository
{
    private readonly IMemoryCache _memoryCache;

    /// <summary>
    /// 添加
    /// </summary>
    /// <param name="order"></param>
    /// <returns></returns>
    public async Task<Guid> AddAsync(OrderEntity order)
    {
        await Task.Delay(1);
        var id = Guid.NewGuid();
        order.OrderId = id;
        _memoryCache.Set(id, order);
        return id;
    }

    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task DeleteAsync(Guid id)
    {
        await Task.Delay(1);
        _memoryCache.Remove(id);
    }

    /// <summary>
    /// 查询
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<OrderEntity?> GetByIdAsync(Guid id)
    {
        await Task.Delay(1);
        return _memoryCache.Get<OrderEntity>(id);
    }
}