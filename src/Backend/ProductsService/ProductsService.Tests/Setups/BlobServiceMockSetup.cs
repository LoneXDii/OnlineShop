using Moq;
using ProductsService.Domain.Abstractions.BlobStorage;

namespace ProductsService.Tests.Setups;

public static class BlobServiceMockSetup
{
    public static void SetupBlobService(this Mock<IBlobService> mock)
    {
        mock.Setup(blobService =>
                blobService.UploadAsync(It.IsAny<Stream>(), It.IsAny<string>()))
            .ReturnsAsync("new-image-url");
        
        mock.Setup(blobService =>
                blobService.DeleteAsync(It.IsAny<string>()))
            .Returns(Task.CompletedTask);
    }
}