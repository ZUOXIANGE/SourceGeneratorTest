using Core.Dtos;
using Microsoft.AspNetCore.Mvc;
using Services;
using SourceGeneratorTest.Infrastructure;

namespace SourceGeneratorTest.Endpoints;

/// <summary>
/// 产品管理端点
/// </summary>
public class ProductEndpoints : IEndpoint
{
    public static void MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/api/product").WithTags("产品管理").WithApiResult();

        group.MapPost("", CreateProductAsync)
            .WithSummary("创建产品")
            .WithDescription("创建一个新产品，返回其唯一标识 ID。");

        group.MapGet("/{id:guid}", GetProductAsync)
            .WithSummary("获取产品")
            .WithDescription("根据产品 ID 获取产品详情。");

        group.MapGet("", GetAllProductsAsync)
            .WithSummary("获取所有产品")
            .WithDescription("返回所有可用产品的信息。");

        group.MapDelete("/{id:guid}", DeleteProductAsync)
            .WithSummary("删除产品")
            .WithDescription("根据产品 ID 删除指定产品。");
    }

    private static Task<Guid> CreateProductAsync(
        [FromBody] CreateProductReq body,
        [FromServices] IProductService productService)
        => productService.CreateProductAsync(body);

    private static Task<ProductDto> GetProductAsync(
        [FromRoute] Guid id,
        [FromServices] IProductService productService)
        => productService.GetProductAsync(id);

    private static Task<List<ProductDto>> GetAllProductsAsync([FromServices] IProductService productService)
        => productService.GetAllProductsAsync();

    private static async Task<bool> DeleteProductAsync(
        [FromRoute] Guid id,
        [FromServices] IProductService productService)
    {
        await productService.DeleteProductAsync(id);
        return true;
    }
}
