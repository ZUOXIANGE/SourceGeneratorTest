using Core.Dtos;

namespace Services;

public interface IFileService
{
    /// <summary>
    /// 保存上传文件并写入元数据
    /// </summary>
    /// <param name="content">文件内容</param>
    /// <param name="fileName">原始文件名</param>
    /// <param name="contentType">内容类型</param>
    /// <param name="cancellationToken"></param>
    /// <returns>文件信息</returns>
    Task<FileDto> UploadAsync(Stream content, string fileName, string contentType, CancellationToken cancellationToken = default);

    /// <summary>
    /// 获取文件元数据
    /// </summary>
    /// <param name="fileId">文件ID</param>
    /// <returns>文件信息，不存在时抛出 <see cref="Core.Exceptions.ApiException"/></returns>
    Task<FileDto> GetAsync(Guid fileId);

    /// <summary>
    /// 获取下载所需信息
    /// </summary>
    /// <param name="fileId">文件ID</param>
    /// <returns>下载信息，元数据或磁盘文件不存在时抛出 <see cref="Core.Exceptions.ApiException"/></returns>
    Task<FileDownload> GetDownloadAsync(Guid fileId);

    /// <summary>
    /// 删除文件及其元数据
    /// </summary>
    /// <param name="fileId">文件ID</param>
    /// <returns>不存在时抛出 <see cref="Core.Exceptions.ApiException"/></returns>
    Task DeleteAsync(Guid fileId);
}

/// <summary>
/// 下载文件信息
/// </summary>
/// <param name="FullPath">磁盘完整路径</param>
/// <param name="FileName">原始文件名</param>
/// <param name="ContentType">内容类型</param>
public record FileDownload(string FullPath, string FileName, string ContentType);
