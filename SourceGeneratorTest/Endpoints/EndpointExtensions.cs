using ServiceScan.SourceGenerator;

namespace SourceGeneratorTest.Endpoints;

/// <summary>
/// 最小 API 端点注册扩展
/// </summary>
public static partial class EndpointExtensions
{
    /// <summary>
    /// 编译时扫描并映射所有实现 <see cref="IEndpoint"/> 的端点
    /// </summary>
    /// <param name="endpoints">端点路由构建器</param>
    /// <returns></returns>
    [ScanForTypes(AssignableTo = typeof(IEndpoint), Handler = nameof(IEndpoint.MapEndpoint))]
    public static partial IEndpointRouteBuilder MapEndpoints(this IEndpointRouteBuilder endpoints);
}
