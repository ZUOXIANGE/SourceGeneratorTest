using Mediator;
using Core.Dtos;

namespace Core.Dtos.Mediator;

/// <summary>
/// 获取所有产品查询请求
/// </summary>
public class GetAllProductsQuery : IRequest<List<ProductDto>>
{
}