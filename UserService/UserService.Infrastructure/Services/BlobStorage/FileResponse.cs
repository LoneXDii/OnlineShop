namespace UserService.Infrastructure.Services.BlobStorage;

public record FileResponse(Stream Stream, string ContentType);