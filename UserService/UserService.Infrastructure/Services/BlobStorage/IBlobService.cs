namespace UserService.Infrastructure.Services.BlobStorage;

public interface IBlobService
{
    Task<Guid> UploadAsync(Stream stream, string contentType);
    Task<FileResponse> DownloadAsync(Guid fileId);
    Task DeleteAsync(Guid fileId);
}
