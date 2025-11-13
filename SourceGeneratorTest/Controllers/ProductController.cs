using Core.Dtos;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace SourceGeneratorTest.Controllers;

/// <summary>
/// 产品管理
/// </summary>
[ApiController]
[Route("api/[controller]")]
[AutoConstruct(GuardSetting.Enabled)]
public partial class ProductController : ControllerBase
{
    private readonly ILogger<ProductController> _logger;
    private readonly IProductService _productService;

    /// <summary>
    /// 创建产品
    /// </summary>
    /// <remarks>创建一个新产品，返回其唯一标识 ID。</remarks>
    /// <param name="req"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<ActionResult<Guid>> CreateProductAsync([FromBody] CreateProductReq req)
    {
        try
        {
            var id = await _productService.CreateProductAsync(req);
            return Ok(id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "创建产品失败");
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// 获取产品
    /// </summary>
    /// <remarks>根据产品 ID 获取产品详情。</remarks>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<ProductDto>> GetProductAsync(Guid id)
    {
        try
        {
            var product = await _productService.GetProductAsync(id);
            return Ok(product);
        }
        catch (ArgumentNullException)
        {
            return NotFound("产品不存在");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "获取产品失败");
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// 获取所有产品
    /// </summary>
    /// <remarks>分页或列表返回所有可用产品的信息。</remarks>
    /// <returns></returns>
    [HttpGet]
    public async Task<ActionResult<List<ProductDto>>> GetAllProductsAsync()
    {
        try
        {
            var products = await _productService.GetAllProductsAsync();
            return Ok(products);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "获取产品列表失败");
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// 删除产品
    /// </summary>
    /// <remarks>根据产品 ID 删除指定产品。</remarks>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteProductAsync(Guid id)
    {
        try
        {
            await _productService.DeleteProductAsync(id);
            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "删除产品失败");
            return BadRequest(ex.Message);
        }
    }
}