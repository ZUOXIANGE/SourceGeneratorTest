using System.ComponentModel;
using NetEscapades.EnumGenerators;
using AutoDbSetGenerators;

namespace Core.Entities;

/// <summary>
/// 订单
/// </summary>
[AutoDbSet(Name = "Orders")]
public class OrderEntity
{
    /// <summary>
    /// 订单id
    /// </summary>
    public Guid OrderId { get; set; }

    /// <summary>
    /// 姓名
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// 电话
    /// </summary>
    public string Phone { get; set; } = string.Empty;

    /// <summary>
    /// 地址
    /// </summary>
    public AddressDto? Address { get; set; }

    /// <summary>
    /// 订单类型
    /// </summary>
    public OrderType OrderType { get; set; }
}

public class AddressDto
{
    /// <summary>
    /// 城市
    /// </summary>
    public string? City { get; set; }

    /// <summary>
    /// 详细地址
    /// </summary>
    public string? Detail { get; set; }

}

[EnumExtensions]
public enum OrderType
{
    /// <summary>
    /// 未知
    /// </summary>
    [Description("未知")]
    Unknown = 0,

    /// <summary>
    /// 租车
    /// </summary>
    [Description("租车")]
    Rental = 1,

    /// <summary>
    /// 打车
    /// </summary>
    [Description("打车")]
    Taxi = 2,

}