using AutoDbSetGenerators;

namespace Core.Entities;

/// <summary>
/// 上传文件记录
/// </summary>
[AutoDbSet(Name = "FileRecords")]
public class FileRecordEntity
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
    /// 存储相对路径
    /// </summary>
    public string StoredPath { get; set; } = string.Empty;

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
    public DateTime UploadedAt { get; set; } = DateTime.Now;
}
