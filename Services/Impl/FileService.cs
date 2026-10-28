using AutoCtor;
using Core.Data;
using Core.Dtos;
using Core.Entities;
using Core.Exceptions;
using Core.Options;
using Injectio;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Services.Impl;

[AutoConstruct]
[RegisterScoped]
public partial class FileService : IFileService
{
    private readonly AppDbContext _dbContext;
    private readonly IOptionsMonitor<FileStorageOptions> _optionsMonitor;
    private readonly ILogger<FileService> _logger;

    /// <inheritdoc />
    public async Task<FileDto> UploadAsync(
        Stream content,
        string fileName,
        string contentType,
        CancellationToken cancellationToken = default)
    {
        var rootPath = _optionsMonitor.CurrentValue.RootPath;
        var fileId = Guid.NewGuid();
        var relativePath = Path.Combine(DateTime.Now.ToString("yyyyMM"), $"{fileId:N}{Path.GetExtension(fileName)}");
        var fullPath = Path.Combine(rootPath, relativePath);

        Directory.CreateDirectory(Path.GetDirectoryName(fullPath)!);

        long size;
        await using (var target = File.Create(fullPath))
        {
            await content.CopyToAsync(target, cancellationToken);
            size = target.Length;
        }

        var entity = new FileRecordEntity
        {
            FileId = fileId,
            FileName = fileName,
            StoredPath = relativePath,
            ContentType = contentType,
            Size = size,
            UploadedAt = DateTime.Now
        };

        _dbContext.FileRecords.Add(entity);
        await _dbContext.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("文件已上传: {FileName} ({Size} 字节)", fileName, size);
        return ToDto(entity);
    }

    /// <inheritdoc />
    public async Task<FileDto> GetAsync(Guid fileId)
    {
        var entity = await _dbContext.FileRecords
            .AsNoTracking()
            .FirstOrDefaultAsync(entity => entity.FileId == fileId);

        return entity is null
            ? throw new ApiException(404, "文件不存在")
            : ToDto(entity);
    }

    /// <inheritdoc />
    public async Task<FileDownload> GetDownloadAsync(Guid fileId)
    {
        var entity = await _dbContext.FileRecords
            .AsNoTracking()
            .FirstOrDefaultAsync(entity => entity.FileId == fileId);
        if (entity is null)
            throw new ApiException(404, "文件不存在");

        var fullPath = Path.Combine(_optionsMonitor.CurrentValue.RootPath, entity.StoredPath);
        if (!File.Exists(fullPath))
            throw new ApiException(404, "文件不存在");

        return new FileDownload(fullPath, entity.FileName, entity.ContentType);
    }

    /// <inheritdoc />
    public async Task DeleteAsync(Guid fileId)
    {
        var entity = await _dbContext.FileRecords
            .FirstOrDefaultAsync(entity => entity.FileId == fileId);
        if (entity is null)
            throw new ApiException(404, "文件不存在");

        _dbContext.FileRecords.Remove(entity);
        await _dbContext.SaveChangesAsync();

        var fullPath = Path.Combine(_optionsMonitor.CurrentValue.RootPath, entity.StoredPath);
        if (File.Exists(fullPath))
            File.Delete(fullPath);

        _logger.LogInformation("文件已删除: {FileId}", fileId);
    }

    private static FileDto ToDto(FileRecordEntity entity) => new()
    {
        FileId = entity.FileId,
        FileName = entity.FileName,
        ContentType = entity.ContentType,
        Size = entity.Size,
        UploadedAt = entity.UploadedAt,
        DownloadUrl = $"/api/file/{entity.FileId}/download"
    };
}
