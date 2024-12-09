using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs;
using ProductsService.Domain.Abstractions.BlobStorage;
using ProductsService.Infrastructure.Configuration;
using Microsoft.Extensions.Options;

namespace ProductsService.Infrastructure.Services;

internal class BlobService : IBlobService
{
    private readonly BlobServiceClient _blobServiceClient;
    private readonly BlobServiceOptions _containerOptions;

    public BlobService(BlobServiceClient blobServiceClient, IOptions<BlobServiceOptions> containerOptions)
    {
        _blobServiceClient = blobServiceClient;
        _containerOptions = containerOptions.Value;
        var containerClient = _blobServiceClient.GetBlobContainerClient(_containerOptions.Container);
        containerClient.CreateIfNotExists(PublicAccessType.Blob);
    }

    public async Task<string> UploadAsync(Stream stream, string contentType)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient(_containerOptions.Container);

        var fileId = Guid.NewGuid();
        var blobClient = containerClient.GetBlobClient(fileId.ToString());

        await blobClient.UploadAsync(stream,
            new BlobHttpHeaders { ContentType = contentType });

        var fileUrl = $"{_containerOptions.BaseUrl}/{_containerOptions.Container}/{fileId}";

        return fileUrl;
    }

    public async Task DeleteAsync(string fileUrl)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient(_containerOptions.Container);

        var fileId = fileUrl.Split("/").Last();
        var blobClient = containerClient.GetBlobClient(fileId);

        await blobClient.DeleteIfExistsAsync();
    }
}
