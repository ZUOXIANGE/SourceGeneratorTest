using Core.Dtos;
using Core.Options;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Services;

namespace SourceGeneratorTest.Controllers;

[ApiController]
[Route("api/[controller]")]
[AutoConstruct(GuardSetting.Enabled)]
public partial class TestController : ControllerBase
{
    private readonly ILogger<TestController> _logger;
    private readonly IOrderService _orderService;
    private readonly IOptionsMonitor<TestOptions> _testOptionsMonitor;
    private readonly IOptionsMonitor<OrderOptions> _orderOptionsMonitor;
    private readonly IOptions<AppSettings> _appSettings;


    [HttpGet("ping")]
    public string Ping()
    {
        _logger.LogInformation("当前配置1:{@options}", _testOptionsMonitor.CurrentValue);
        _logger.LogInformation("当前配置2:{@options}", _orderOptionsMonitor.CurrentValue);
        _logger.LogInformation("当前配置3:{@options}", _appSettings.Value);

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