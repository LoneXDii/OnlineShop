using Azure.Storage.Blobs;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Testcontainers.Azurite;
using UserService.DAL.Models;
using UserService.DAL.Services.BlobStorage;

namespace UserService.Tests.IntegrationTests.Services;

public class BlobServiceTests: IAsyncLifetime
{
    private readonly AzuriteContainer _azuriteContainer;
    private  BlobServiceClient _blobServiceClient;
    private  BlobServiceOptions _blobServiceOptions;
    
    public BlobServiceTests()
    {
        _azuriteContainer = new AzuriteBuilder()
            .WithImage("mcr.microsoft.com/azure-storage/azurite:latest")
            .Build();
    }

    public async Task InitializeAsync()
    {
        await _azuriteContainer.StartAsync();
    }

    public async Task DisposeAsync()
    {
        await _azuriteContainer.StopAsync();
    } 
    
    [Fact]
    public async Task UploadAsync_WhenCalled_ShouldUploadFileAndReturnUrl()
    {
        //Arrange
        var testStream = new MemoryStream([1, 2, 3, 4, 5]);
        var testContentType = "text/plain";
        var blobService = GetBlobService();
        
        //Act
        var fileUrl = await blobService.UploadAsync(testStream, testContentType);
        
        var containerClient = _blobServiceClient.GetBlobContainerClient(_blobServiceOptions.Container);
        var fileName = fileUrl.Split('/').Last();
        var blobClient = containerClient.GetBlobClient(fileName);

        var exists = await blobClient.ExistsAsync();
        
        //Assert
        Assert.StartsWith($"{_blobServiceOptions.BaseUrl}/{_blobServiceOptions.Container}/", fileUrl);
        Assert.True(exists.Value);
    }
    
    [Fact]
    public async Task DeleteAsync_Should_DeleteFileFromBlob()
    {
        //Arrange
        var testStream = new MemoryStream(new byte[] { 1, 2, 3, 4, 5 });
        var testContentType = "text/plain";
        var blobService = GetBlobService();
        
        var fileUrl = await blobService.UploadAsync(testStream, testContentType);

        //Act
        await blobService.DeleteAsync(fileUrl);

        var containerClient = _blobServiceClient.GetBlobContainerClient(_blobServiceOptions.Container);
        var fileName = fileUrl.Split('/').Last();
        var blobClient = containerClient.GetBlobClient(fileName);

        var exists = await blobClient.ExistsAsync();
        
        //Assert
        Assert.False(exists);
    }
    
    private IBlobService GetBlobService()
    {
        _blobServiceClient = new BlobServiceClient(_azuriteContainer.GetConnectionString());
        
        _blobServiceOptions  = new BlobServiceOptions
        {
            Container = "container",
            BaseUrl = $"{_azuriteContainer.GetBlobEndpoint()}"
        };
        
        var loggerMock = new Mock<ILogger<BlobService>>();
        
        return new BlobService(_blobServiceClient, Options.Create(_blobServiceOptions), loggerMock.Object);
    }
}