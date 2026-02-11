using Core.Dtos;
using Core.Dtos.Mediator;
using Mediator;
using Microsoft.AspNetCore.Mvc;


namespace SourceGeneratorTest.Controllers;

/// <summary>
/// Mediator 演示控制器
/// </summary>
[ApiController]
[Route("api/[controller]")]
[AutoConstruct]
public partial class MediatorController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<MediatorController> _logger;

    /// <summary>
    /// 通过 Mediator 创建产品
    /// </summary>
    /// <param name="command">创建产品命令</param>
    /// <returns>产品ID</returns>
    [HttpPost("products")]
    public async Task<ActionResult<Guid>> CreateProductAsync([FromBody] CreateProductCommand command)
    {
        try
        {
            _logger.LogInformation("Creating product via Mediator: {ProductName}", command.Name);
            var productId = await _mediator.Send(command);
            return Ok(productId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create product via Mediator");
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// 通过 Mediator 获取产品
    /// </summary>
    /// <param name="id">产品ID</param>
    /// <returns>产品信息</returns>
    [HttpGet("products/{id}")]
    public async Task<ActionResult<ProductDto>> GetProductAsync(Guid id)
    {
        try
        {
            _logger.LogInformation("Getting product via Mediator: {ProductId}", id);
            var query = new GetProductQuery(id);
            var product = await _mediator.Send(query);
            return Ok(product);
        }
        catch (ArgumentNullException)
        {
            return NotFound("产品不存在");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get product via Mediator: {ProductId}", id);
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// 通过 Mediator 获取所有产品
    /// </summary>
    /// <returns>产品列表</returns>
    [HttpGet("products")]
    public async Task<ActionResult<List<ProductDto>>> GetAllProductsAsync()
    {
        try
        {
            _logger.LogInformation("Getting all products via Mediator");
            var query = new GetAllProductsQuery();
            var products = await _mediator.Send(query);
            return Ok(products);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get all products via Mediator");
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Mediator 健康检查
    /// </summary>
    /// <remarks>检查 Mediator 集成是否正常工作，返回健康状态。</remarks>
    /// <returns>状态信息</returns>
    [HttpGet("health")]
    public ActionResult<object> HealthCheck()
    {
        return Ok(new
        {
            Status = "Healthy",
            Message = "Mediator integration is working properly",
            Timestamp = DateTime.UtcNow
        });
    }

    /// <summary>
    /// 发送广播通知示例
    /// </summary>
    /// <param name="request">广播请求</param>
    /// <returns></returns>
    [HttpPost("broadcast-notification")]
    public async Task<IActionResult> BroadcastNotification(
        [FromBody] BroadcastRequest request)
    {
        var notification = new ProductCreatedNotification(request.ProductId, request.Name, request.Price);
        await _mediator.Publish(notification);

        return Ok(new { Message = "广播通知已发送", Timestamp = DateTime.UtcNow });
    }
}

/// <summary>
/// 广播请求模型
/// </summary>
public class BroadcastRequest
{
    public Guid ProductId { get; set; }

    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
}