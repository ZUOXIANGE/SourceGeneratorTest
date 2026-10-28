using Core.Dtos;
using Core.Dtos.Mappers;
using Core.Entities;
using Services;
using SourceGeneratorTest.Infrastructure;

namespace SourceGeneratorTest.Endpoints;

/// <summary>
/// 数据库演示端点
/// </summary>
public class DatabaseDemoEndpoints : IEndpoint
{
    public static void MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/api/databaseDemo").WithTags("数据库演示").WithApiResult();

        #region 产品相关API

        group.MapGet("/products", GetAllProductsAsync)
            .WithSummary("获取所有产品")
            .WithDescription("返回所有产品实体列表，用于演示数据库访问。");

        group.MapGet("/products/{id:guid}", GetProductAsync)
            .WithSummary("根据ID获取产品")
            .WithDescription("根据产品唯一标识 ID 查询产品实体详情。");

        group.MapPost("/products", CreateProductAsync)
            .WithSummary("创建产品")
            .WithDescription("创建一个产品实体并返回其详情。");

        group.MapGet("/products/category/{category}", GetProductsByCategoryAsync)
            .WithSummary("根据分类获取产品")
            .WithDescription("根据枚举分类筛选产品列表（含中文枚举说明）。");

        #endregion

        #region 订单相关API

        group.MapGet("/orders", GetAllOrdersAsync)
            .WithSummary("获取所有订单")
            .WithDescription("返回所有订单实体列表，用于演示数据库访问。");

        group.MapGet("/orders/{id:guid}", GetOrderAsync)
            .WithSummary("根据ID获取订单")
            .WithDescription("根据订单唯一标识 ID 查询订单实体详情。");

        group.MapPost("/orders", CreateOrderAsync)
            .WithSummary("创建订单")
            .WithDescription("创建一个订单实体并返回其详情。");

        group.MapGet("/orders/type/{orderType}", GetOrdersByTypeAsync)
            .WithSummary("根据类型获取订单")
            .WithDescription("根据订单类型筛选订单列表（含中文枚举说明）。");

        #endregion
    }

    private static Task<List<ProductEntity>> GetAllProductsAsync(IProductService productService)
        => productService.GetAllProductEntitiesAsync();

    private static async Task<ProductEntity> GetProductAsync(Guid id, IProductService productService)
    {
        var product = await productService.GetProductByIdAsync(id);
        return product ?? throw new ApiException(StatusCodes.Status404NotFound, $"未找到ID为{id}的产品");
    }

    private static Task<ProductEntity> CreateProductAsync(CreateProductReq request, IProductService productService)
        => productService.CreateProductEntityAsync(request.ToEntity());

    private static Task<List<ProductEntity>> GetProductsByCategoryAsync(ProductCategory category, IProductService productService)
        => productService.GetProductsByCategoryAsync(category);

    private static Task<List<OrderEntity>> GetAllOrdersAsync(IOrderService orderService)
        => orderService.GetAllOrdersAsync();

    private static async Task<OrderEntity> GetOrderAsync(Guid id, IOrderService orderService)
    {
        var order = await orderService.GetOrderByIdAsync(id);
        return order ?? throw new ApiException(StatusCodes.Status404NotFound, $"未找到ID为{id}的订单");
    }

    private static Task<OrderEntity> CreateOrderAsync(CreateOrderReq request, IOrderService orderService)
        => orderService.CreateOrderEntityAsync(request.ToEntity());

    private static Task<List<OrderEntity>> GetOrdersByTypeAsync(OrderType orderType, IOrderService orderService)
        => orderService.GetOrdersByTypeAsync(orderType);
}
