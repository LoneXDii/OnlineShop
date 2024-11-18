using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs;
using ProductsService.Domain.Abstractions.BlobStorage;

namespace ProductsService.Infrastructure.Services;

internal class BlobService : IBlobService
{
	private readonly BlobServiceClient _blobServiceClient;
	private const string _containerName = "products";

	public BlobService(BlobServiceClient blobServiceClient)
	{
		_blobServiceClient = blobServiceClient;
		var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
		containerClient.CreateIfNotExists(PublicAccessType.Blob);
	}

	public async Task<string> UploadAsync(Stream stream, string contentType)
	{
		var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);

		var fileId = Guid.NewGuid();
		var blobClient = containerClient.GetBlobClient(fileId.ToString());

		await blobClient.UploadAsync(stream,
			new BlobHttpHeaders { ContentType = contentType });

		//Later will be replace by Ocelot endpoint
		var fileUrl = $"http://127.0.0.1:10000/devstoreaccount1/avatars/{fileId}";

		return fileUrl;
	}

	public async Task DeleteAsync(string fileUrl)
	{
		var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);

		var fileId = fileUrl.Split("/").Last();
		var blobClient = containerClient.GetBlobClient(fileId);

		await blobClient.DeleteIfExistsAsync();
	}
}
