using Core.Entities;

namespace Core.Dtos;

public class OrderDto
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
    public string? OrderTypeDesc { get; set; }
}