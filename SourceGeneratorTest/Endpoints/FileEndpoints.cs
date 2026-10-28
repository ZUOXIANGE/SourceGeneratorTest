using Core.Dtos;
using Core.Exceptions;
using Core.Options;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Services;
using SourceGeneratorTest.Infrastructure;

namespace SourceGeneratorTest.Endpoints;

/// <summary>
/// 文件上传下载端点
/// </summary>
public class FileEndpoints : IEndpoint
{
    private const long BytesPerMB = 1024L * 1024L;

    public static void MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/api/file").WithTags("文件管理").WithApiResult();

        group.MapPost("/upload", UploadAsync)
            .WithSummary("上传文件")
            .WithDescription("以 multipart/form-data 上传单个文件，返回文件元数据与下载地址。")
            .DisableAntiforgery();

        group.MapGet("/{id:guid}/download", DownloadAsync)
            .WithSummary("下载文件")
            .WithDescription("按文件 ID 下载原始文件内容，支持 Range 请求；文件不存在时返回业务状态码 404。")
            .Produces<byte[]>(StatusCodes.Status200OK, contentType: "application/octet-stream");

        group.MapGet("/{id:guid}", GetMetadataAsync)
            .WithSummary("获取文件信息")
            .WithDescription("根据文件 ID 获取文件元数据。");

        group.MapDelete("/{id:guid}", DeleteAsync)
            .WithSummary("删除文件")
            .WithDescription("根据文件 ID 删除文件及其元数据。");
    }

    private static async Task<FileDto> UploadAsync(
        [FromForm] IFormFile file,
        [FromServices] IFileService fileService,
        [FromServices] IOptionsMonitor<FileStorageOptions> optionsMonitor,
        CancellationToken cancellationToken)
    {
        var options = optionsMonitor.CurrentValue;
        if (file.Length == 0)
            throw new ApiException(StatusCodes.Status400BadRequest, "文件不能为空");

        var fileName = Path.GetFileName(file.FileName);
        var extension = Path.GetExtension(fileName);
        if (!options.AllowedExtensions.Contains(extension, StringComparer.OrdinalIgnoreCase))
        {
            var displayExtension = string.IsNullOrEmpty(extension) ? "无扩展名" : extension;
            throw new ApiException(StatusCodes.Status400BadRequest, $"不支持的文件类型：{displayExtension}");
        }

        if (file.Length > options.MaxSizeMB * BytesPerMB)
            throw new ApiException(StatusCodes.Status400BadRequest, $"文件大小不能超过 {options.MaxSizeMB}MB");

        await using var stream = file.OpenReadStream();
        return await fileService.UploadAsync(stream, fileName, file.ContentType, cancellationToken);
    }

    private static async Task<IResult> DownloadAsync(
        [FromRoute] Guid id,
        [FromServices] IFileService fileService)
    {
        var download = await fileService.GetDownloadAsync(id);

        return TypedResults.PhysicalFile(
            download.FullPath,
            download.ContentType,
            download.FileName,
            enableRangeProcessing: true);
    }

    private static Task<FileDto> GetMetadataAsync(
        [FromRoute] Guid id,
        [FromServices] IFileService fileService)
        => fileService.GetAsync(id);

    private static async Task<bool> DeleteAsync(
        [FromRoute] Guid id,
        [FromServices] IFileService fileService)
    {
        await fileService.DeleteAsync(id);
        return true;
    }
}
