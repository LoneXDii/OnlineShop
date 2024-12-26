using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs;
using ProductsService.Domain.Abstractions.BlobStorage;
using ProductsService.Infrastructure.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;

namespace ProductsService.Infrastructure.Services;

internal class BlobService : IBlobService
{
    private readonly BlobServiceClient _blobServiceClient;
    private readonly BlobServiceOptions _containerOptions;
    private readonly ILogger<BlobService> _logger;

    public BlobService(BlobServiceClient blobServiceClient, IOptions<BlobServiceOptions> containerOptions, ILogger<BlobService> logger)
    {
        _blobServiceClient = blobServiceClient;
        _containerOptions = containerOptions.Value;
        _logger = logger;
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

        _logger.LogInformation($"Uploading image: {fileUrl} to blob storage");

        return fileUrl;
    }

    public async Task DeleteAsync(string fileUrl)
    {
        _logger.LogInformation($"Deleting image: {fileUrl} from blob storage");

        var containerClient = _blobServiceClient.GetBlobContainerClient(_containerOptions.Container);

        var fileId = fileUrl.Split("/").Last();
        var blobClient = containerClient.GetBlobClient(fileId);

        await blobClient.DeleteIfExistsAsync();
    }
}
