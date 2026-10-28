using Core.Dtos;

namespace SourceGeneratorTest.Infrastructure;

/// <summary>
/// 端点过滤器：把处理函数的返回值包装成统一响应，HTTP 状态码固定为 200
/// </summary>
public sealed class ApiResultFilter : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var result = await next(context);

        // 处理函数已自行返回 IResult 时不再包装
        if (result is IResult)
            return result;

        return Results.Ok(ApiResult<object?>.Success(result));
    }
}

/// <summary>
/// 统一响应相关扩展
/// </summary>
public static class ApiResultEndpointExtensions
{
    /// <summary>
    /// 为端点分组启用统一响应包装
    /// </summary>
    /// <param name="group">端点分组</param>
    public static RouteGroupBuilder WithApiResult(this RouteGroupBuilder group)
        => group.AddEndpointFilter<ApiResultFilter>();
}
