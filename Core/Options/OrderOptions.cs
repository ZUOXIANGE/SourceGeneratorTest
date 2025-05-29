namespace Core.Options;

[Option(nameof(OrderOptions))]
public record OrderOptions //配置建议使用Record
{
    /// <summary>
    /// 数量
    /// </summary>
    public int Count { get; set; }
};