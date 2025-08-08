using System.ComponentModel.DataAnnotations;
using Core.Entities;

namespace Core.Dtos;

/// <summary>
/// 创建产品请求
/// </summary>
public class CreateProductReq
{
    /// <summary>
    /// 产品名称
    /// </summary>
    [Required]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// 产品描述
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// 价格
    /// </summary>
    [Range(0, double.MaxValue, ErrorMessage = "价格必须大于等于0")]
    public decimal Price { get; set; }

    /// <summary>
    /// 产品类别
    /// </summary>
    public ProductCategory Category { get; set; }

    /// <summary>
    /// 是否可用
    /// </summary>
    public bool IsAvailable { get; set; } = true;
}