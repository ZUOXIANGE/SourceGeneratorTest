using System.ComponentModel;
using Core.Dtos;
using Core.Entities;
using Core.Exceptions;
using ModelContextProtocol;
using ModelContextProtocol.Server;
using Services;

namespace SourceGeneratorTest.Mcp;

/// <summary>
/// 产品管理 MCP 工具，复用 <see cref="IProductService"/> 业务逻辑
/// </summary>
[McpServerToolType]
public sealed class ProductMcpTools(IProductService productService)
{
    /// <summary>
    /// 创建产品
    /// </summary>
    [McpServerTool(Name = "create_product"), Description("创建一个新产品，返回其唯一标识 ID。")]
    public Task<Guid> CreateProductAsync(
        [Description("产品名称")] string name,
        [Description("价格，需大于等于 0")] decimal price,
        [Description("产品类别：Uncategorized=未分类, Electronics=电子产品, Clothing=服装, Food=食品, Books=图书")] ProductCategory category,
        [Description("产品描述")] string? description = null,
        [Description("是否可用，默认 true")] bool isAvailable = true)
        => productService.CreateProductAsync(new CreateProductReq
        {
            Name = name,
            Description = description,
            Price = price,
            Category = category,
            IsAvailable = isAvailable
        });

    /// <summary>
    /// 获取产品
    /// </summary>
    [McpServerTool(Name = "get_product"), Description("根据产品 ID 获取产品详情。")]
    public async Task<ProductDto> GetProductAsync([Description("产品 ID")] Guid productId)
    {
        try
        {
            return await productService.GetProductAsync(productId);
        }
        catch (ApiException ex)
        {
            // 转成 MCP 业务错误，让客户端能看到具体原因（如“产品不存在”）
            throw new McpException(ex.Message);
        }
    }

    /// <summary>
    /// 获取所有产品
    /// </summary>
    [McpServerTool(Name = "list_products"), Description("获取所有可用产品的信息。")]
    public Task<List<ProductDto>> ListProductsAsync()
        => productService.GetAllProductsAsync();

    /// <summary>
    /// 删除产品
    /// </summary>
    [McpServerTool(Name = "delete_product"), Description("根据产品 ID 删除指定产品（软删除）。")]
    public async Task<bool> DeleteProductAsync([Description("产品 ID")] Guid productId)
    {
        await productService.DeleteProductAsync(productId);
        return true;
    }
}
