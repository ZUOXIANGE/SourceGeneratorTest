namespace SourceGeneratorTest.Infrastructure;

/// <summary>
/// 业务异常，由全局异常中间件转换成统一响应
/// </summary>
/// <param name="code">业务状态码</param>
/// <param name="message">提示信息</param>
public sealed class ApiException(int code, string message) : Exception(message)
{
    /// <summary>
    /// 业务状态码
    /// </summary>
    public int Code { get; } = code;
}
