using AutoCtor;
using Core.Data;
using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Repositories.Impl;

[AutoConstruct]
public partial class OrderRepository : IOrderRepository
{
    private readonly AppDbContext _context;

    /// <summary>
    /// 添加
    /// </summary>
    /// <param name="order"></param>
    /// <returns></returns>
    public async Task<Guid> AddAsync(OrderEntity order)
    {
        var id = Guid.NewGuid();
        order.OrderId = id;
        _context.Orders.Add(order);
        await _context.SaveChangesAsync();
        return id;
    }

    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task DeleteAsync(Guid id)
    {
        var order = await _context.Orders.FindAsync(id);
        if (order != null)
        {
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
        }
    }

    /// <summary>
    /// 查询
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<OrderEntity?> GetByIdAsync(Guid id)
    {
        return await _context.Orders.FindAsync(id);
    }

    /// <summary>
    /// 获取所有订单
    /// </summary>
    /// <returns></returns>
    public async Task<List<OrderEntity>> GetAllAsync()
    {
        return await _context.Orders
            .OrderByDescending(o => o.OrderId)
            .ToListAsync();
    }

    /// <summary>
    /// 创建订单实体
    /// </summary>
    /// <param name="order"></param>
    /// <returns></returns>
    public async Task<OrderEntity> CreateAsync(OrderEntity order)
    {
        order.OrderId = Guid.NewGuid();
        _context.Orders.Add(order);
        await _context.SaveChangesAsync();
        return order;
    }

    /// <summary>
    /// 更新订单
    /// </summary>
    /// <param name="order"></param>
    /// <returns></returns>
    public async Task<OrderEntity> UpdateAsync(OrderEntity order)
    {
        _context.Orders.Update(order);
        await _context.SaveChangesAsync();
        return order;
    }

    /// <summary>
    /// 根据订单类型获取订单
    /// </summary>
    /// <param name="orderType"></param>
    /// <returns></returns>
    public async Task<List<OrderEntity>> GetByTypeAsync(OrderType orderType)
    {
        return await _context.Orders
            .Where(o => o.OrderType == orderType)
            .OrderByDescending(o => o.OrderId)
            .ToListAsync();
    }

    /// <summary>
    /// 根据电话号码获取订单
    /// </summary>
    /// <param name="phone"></param>
    /// <returns></returns>
    public async Task<List<OrderEntity>> GetByPhoneAsync(string phone)
    {
        return await _context.Orders
            .Where(o => o.Phone == phone)
            .OrderByDescending(o => o.OrderId)
            .ToListAsync();
    }

    /// <summary>
    /// 删除订单（返回是否成功）
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<bool> DeleteByIdAsync(Guid id)
    {
        var order = await _context.Orders.FindAsync(id);
        if (order == null)
            return false;

        _context.Orders.Remove(order);
        await _context.SaveChangesAsync();
        return true;
    }
}