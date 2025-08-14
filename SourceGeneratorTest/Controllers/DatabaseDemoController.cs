using Microsoft.AspNetCore.Mvc;
using Services;
using Core.Entities;
using Core.Dtos;
using Core.Dtos.Mappers;

namespace SourceGeneratorTest.Controllers;

/// <summary>
/// 数据库演示控制器
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class DatabaseDemoController : ControllerBase
{
    private readonly IProductService _productService;
    private readonly IOrderService _orderService;
    private readonly ILogger<DatabaseDemoController> _logger;

    public DatabaseDemoController(
        IProductService productService,
        IOrderService orderService,
        ILogger<DatabaseDemoController> logger)
    {
        _productService = productService;
        _orderService = orderService;
        _logger = logger;
    }

    #region 产品相关API

    /// <summary>
    /// 获取所有产品
    /// </summary>
    [HttpGet("products")]
    public async Task<ActionResult<List<ProductEntity>>> GetAllProducts()
    {
        try
        {
            var products = await _productService.GetAllProductEntitiesAsync();
            return Ok(products);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "获取产品列表失败");
            return StatusCode(500, "获取产品列表失败");
        }
    }

    /// <summary>
    /// 根据ID获取产品
    /// </summary>
    [HttpGet("products/{id}")]
    public async Task<ActionResult<ProductEntity>> GetProduct(Guid id)
    {
        try
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
                return NotFound($"未找到ID为{id}的产品");

            return Ok(product);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "获取产品失败，ID: {ProductId}", id);
            return StatusCode(500, "获取产品失败");
        }
    }

    /// <summary>
    /// 创建产品
    /// </summary>
    [HttpPost("products")]
    public async Task<ActionResult<ProductEntity>> CreateProduct([FromBody] CreateProductReq request)
    {
        try
        {
            var product = request.ToEntity();

            var createdProduct = await _productService.CreateProductEntityAsync(product);
            return CreatedAtAction(nameof(GetProduct), new { id = createdProduct.ProductId }, createdProduct);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "创建产品失败");
            return StatusCode(500, "创建产品失败");
        }
    }

    /// <summary>
    /// 根据分类获取产品
    /// </summary>
    [HttpGet("products/category/{category}")]
    public async Task<ActionResult<List<ProductEntity>>> GetProductsByCategory(ProductCategory category)
    {
        try
        {
            var products = await _productService.GetProductsByCategoryAsync(category);
            return Ok(products);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "根据分类获取产品失败，分类: {Category}", category);
            return StatusCode(500, "获取产品失败");
        }
    }

    #endregion

    #region 订单相关API

    /// <summary>
    /// 获取所有订单
    /// </summary>
    [HttpGet("orders")]
    public async Task<ActionResult<List<OrderEntity>>> GetAllOrders()
    {
        try
        {
            var orders = await _orderService.GetAllOrdersAsync();
            return Ok(orders);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "获取订单列表失败");
            return StatusCode(500, "获取订单列表失败");
        }
    }

    /// <summary>
    /// 根据ID获取订单
    /// </summary>
    [HttpGet("orders/{id}")]
    public async Task<ActionResult<OrderEntity>> GetOrder(Guid id)
    {
        try
        {
            var order = await _orderService.GetOrderByIdAsync(id);
            if (order == null)
                return NotFound($"未找到ID为{id}的订单");

            return Ok(order);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "获取订单失败，ID: {OrderId}", id);
            return StatusCode(500, "获取订单失败");
        }
    }

    /// <summary>
    /// 创建订单
    /// </summary>
    [HttpPost("orders")]
    public async Task<ActionResult<OrderEntity>> CreateOrder([FromBody] CreateOrderReq request)
    {
        try
        {
            var order = request.ToEntity();

            var createdOrder = await _orderService.CreateOrderEntityAsync(order);
            return CreatedAtAction(nameof(GetOrder), new { id = createdOrder.OrderId }, createdOrder);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "创建订单失败");
            return StatusCode(500, "创建订单失败");
        }
    }

    /// <summary>
    /// 根据类型获取订单
    /// </summary>
    [HttpGet("orders/type/{orderType}")]
    public async Task<ActionResult<List<OrderEntity>>> GetOrdersByType(OrderType orderType)
    {
        try
        {
            var orders = await _orderService.GetOrdersByTypeAsync(orderType);
            return Ok(orders);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "根据类型获取订单失败，类型: {OrderType}", orderType);
            return StatusCode(500, "获取订单失败");
        }
    }

    #endregion
}