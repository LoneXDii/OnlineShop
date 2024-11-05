using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs;

namespace UserService.Infrastructure.Services.BlobStorage;

internal class BlobService : IBlobService
{
    private readonly BlobServiceClient _blobServiceClient;
    private const string _containerName = "avatars";

    public BlobService(BlobServiceClient blobServiceClient)
    {
        _blobServiceClient = blobServiceClient;
        var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
        containerClient.CreateIfNotExists(PublicAccessType.Blob);
    }

    public async Task<Guid> UploadAsync(Stream stream, string contentType)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);

        var fileId = Guid.NewGuid();
        var blobClient = containerClient.GetBlobClient(fileId.ToString());

        await blobClient.UploadAsync(stream,
            new BlobHttpHeaders { ContentType = contentType });

        return fileId;
    }

    public async Task<FileResponse> DownloadAsync(Guid fileId)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);

        var blobClient = containerClient.GetBlobClient(fileId.ToString());
        var response = await blobClient.DownloadContentAsync();

        return new FileResponse(response.Value.Content.ToStream(), response.Value.Details.ContentType);
    }

    public async Task DeleteAsync(Guid fileId)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);

        var blobClient = containerClient.GetBlobClient(fileId.ToString());

        await blobClient.DeleteIfExistsAsync();
    }
}
