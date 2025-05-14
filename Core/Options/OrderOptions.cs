namespace Core.Options;

[Option(nameof(OrderOptions))]
public record OrderOptions
{
    /// <summary>
    /// 数量
    /// </summary>
    public int Count { get; set; }
};