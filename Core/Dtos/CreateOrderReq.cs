using System.ComponentModel.DataAnnotations;
using Core.Entities;

namespace Core.Dtos;

public class CreateOrderReq
{
    /// <summary>
    /// 姓名
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// 电话
    /// </summary>
    [Required]
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