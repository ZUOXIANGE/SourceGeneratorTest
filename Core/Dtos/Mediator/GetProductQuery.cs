using Mediator;

namespace Core.Dtos.Mediator;

/// <summary>
/// 获取产品查询请求
/// </summary>
public class GetProductQuery : IRequest<ProductDto>
{
    /// <summary>
    /// 产品ID
    /// </summary>
    public Guid ProductId { get; set; }

    public GetProductQuery(Guid productId)
    {
        ProductId = productId;
    }
}