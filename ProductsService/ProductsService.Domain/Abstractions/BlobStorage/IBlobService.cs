namespace ProductsService.Domain.Abstractions.BlobStorage;

public interface IBlobService
{
    Task<string> UploadAsync(Stream stream, string contentType);
    Task DeleteAsync(string fileUrl);
}
