using System.ComponentModel.DataAnnotations;
using Mediator;
using Core.Entities;

namespace Core.Dtos.Mediator;

/// <summary>
/// 创建产品命令
/// </summary>
public class CreateProductCommand : IRequest<Guid>
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
}