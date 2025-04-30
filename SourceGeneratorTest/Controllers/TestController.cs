using Core.Dtos;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace SourceGeneratorTest.Controllers;

[ApiController]
[Route("[controller]")]
[AutoConstruct(GuardSetting.Enabled)]
public partial class TestController : ControllerBase
{
    private readonly ILogger<TestController> _logger;
    private readonly IOrderService _orderService;

    [HttpGet("ping")]
    public string Ping()
    {
        _logger.LogInformation("测试请求");
        return "pong";
    }

    /// <summary>
    /// 创建订单
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    [HttpPost("createOrder")]
    public async Task<Guid> CreateOrderAsync([FromBody] CreateOrderReq req)
    {
        return await _orderService.CreateOrderAsync(req);
    }

    /// <summary>
    /// 获取订单
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("getOrder")]
    public async Task<OrderDto> GetOrderAsync(Guid id)
    {
        return await _orderService.GetOrderAsync(id);
    }
}