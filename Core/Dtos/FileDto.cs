namespace Core.Dtos;

/// <summary>
/// 文件信息
/// </summary>
public class FileDto
{
    /// <summary>
    /// 文件ID
    /// </summary>
    public Guid FileId { get; set; }

    /// <summary>
    /// 原始文件名
    /// </summary>
    public string FileName { get; set; } = string.Empty;

    /// <summary>
    /// 内容类型
    /// </summary>
    public string ContentType { get; set; } = string.Empty;

    /// <summary>
    /// 文件大小（字节）
    /// </summary>
    public long Size { get; set; }

    /// <summary>
    /// 上传时间
    /// </summary>
    public DateTime UploadedAt { get; set; }

    /// <summary>
    /// 下载地址
    /// </summary>
    public string DownloadUrl { get; set; } = string.Empty;
}
