namespace SourceGeneratorTest.Endpoints;

/// <summary>
/// 最小 API 端点定义，实现该接口的类型会被自动扫描并映射
/// </summary>
public interface IEndpoint
{
    /// <summary>
    /// 映射端点
    /// </summary>
    /// <param name="endpoints">端点路由构建器</param>
    static abstract void MapEndpoint(IEndpointRouteBuilder endpoints);
}
