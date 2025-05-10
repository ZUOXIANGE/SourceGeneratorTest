using Core.Dtos;
using Core.Options;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Services;

namespace SourceGeneratorTest.Controllers;

[ApiController]
[Route("[controller]")]
[AutoConstruct(GuardSetting.Enabled)]
public partial class TestController : ControllerBase
{
    private readonly ILogger<TestController> _logger;
    private readonly IOrderService _orderService;
    private readonly IOptionsMonitor<TestOptions> _testOptionsMonitor;

    [HttpGet("ping")]
    public string Ping()
    {
        _logger.LogInformation("当前配置:{@options}", _testOptionsMonitor.CurrentValue);
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