namespace Core.Dtos;

/// <summary>
/// 统一响应结果封装，HTTP 状态码固定为 200，业务状态由 Code 字段表示
/// </summary>
/// <typeparam name="T">业务数据类型</typeparam>
public record ApiResult<T>
{
    /// <summary>
    /// 业务成功状态码
    /// </summary>
    public const int SuccessCode = 200;

    /// <summary>
    /// 业务状态码，200 表示成功
    /// </summary>
    public int Code { get; init; }

    /// <summary>
    /// 业务数据
    /// </summary>
    public T? Data { get; init; }

    /// <summary>
    /// 提示信息
    /// </summary>
    public string Msg { get; init; } = string.Empty;

    /// <summary>
    /// 创建成功结果
    /// </summary>
    /// <param name="data">业务数据</param>
    /// <param name="msg">提示信息</param>
    public static ApiResult<T> Success(T data, string msg = "成功")
        => new() { Code = SuccessCode, Data = data, Msg = msg };

    /// <summary>
    /// 创建失败结果
    /// </summary>
    /// <param name="code">业务状态码</param>
    /// <param name="msg">提示信息</param>
    public static ApiResult<T> Failure(int code, string msg)
        => new() { Code = code, Data = default, Msg = msg };
}
