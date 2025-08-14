using System.ComponentModel;
using NetEscapades.EnumGenerators;
using AutoDbSetGenerators;

namespace Core.Entities;

/// <summary>
/// 产品实体
/// </summary>
[AutoDbSet(Name = "Products")]
public class ProductEntity
{
    /// <summary>
    /// 产品ID
    /// </summary>
    public Guid ProductId { get; set; }

    /// <summary>
    /// 产品名称
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// 产品描述
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// 价格
    /// </summary>
    public decimal Price { get; set; }

    /// <summary>
    /// 产品类别
    /// </summary>
    public ProductCategory Category { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    /// <summary>
    /// 是否可用
    /// </summary>
    public bool IsAvailable { get; set; } = true;
}

[EnumExtensions]
public enum ProductCategory
{
    /// <summary>
    /// 未分类
    /// </summary>
    [Description("未分类")]
    Uncategorized = 0,

    /// <summary>
    /// 电子产品
    /// </summary>
    [Description("电子产品")]
    Electronics = 1,

    /// <summary>
    /// 服装
    /// </summary>
    [Description("服装")]
    Clothing = 2,

    /// <summary>
    /// 食品
    /// </summary>
    [Description("食品")]
    Food = 3,

    /// <summary>
    /// 图书
    /// </summary>
    [Description("图书")]
    Books = 4
}