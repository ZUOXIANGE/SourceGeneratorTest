using Core.Dtos;
using Core.Options;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Services;

namespace SourceGeneratorTest.Controllers;

/// <summary>
/// 测试
/// </summary>
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

    /// <summary>
    /// Ping 测试接口
    /// </summary>
    /// <remarks>用于测试服务可用性，返回字符串 'pong'。</remarks>
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
    /// <remarks>创建一个新订单，返回生成的订单 ID。</remarks>
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
    /// <remarks>根据订单 ID 获取订单详情。</remarks>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("getOrder")]
    public async Task<OrderDto> GetOrderAsync(Guid id)
    {
        return await _orderService.GetOrderAsync(id);
    }
}