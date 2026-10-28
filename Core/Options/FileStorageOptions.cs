namespace Core.Options;

/// <summary>
/// 文件存储配置
/// </summary>
[Option(nameof(FileStorageOptions))]
public class FileStorageOptions
{
    /// <summary>
    /// 存储根目录，相对路径会以应用程序内容根目录为基准
    /// </summary>
    public string RootPath { get; set; } = "App_Data/uploads";

    /// <summary>
    /// 单个文件大小上限（MB）
    /// </summary>
    public int MaxSizeMB { get; set; } = 20;

    /// <summary>
    /// 允许上传的扩展名
    /// </summary>
    public string[] AllowedExtensions { get; set; } =
    [
        ".txt", ".pdf", ".png", ".jpg", ".jpeg", ".zip", ".docx", ".xlsx"
    ];
}
