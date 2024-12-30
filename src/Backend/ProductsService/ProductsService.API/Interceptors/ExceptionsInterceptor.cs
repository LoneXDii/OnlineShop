using Grpc.Core;
using Grpc.Core.Interceptors;

namespace ProductsService.API.Interceptors;

public class ExceptionsInterceptor : Interceptor
{
    private readonly ILogger<ExceptionsInterceptor> _logger;

    public ExceptionsInterceptor(ILogger<ExceptionsInterceptor> logger)
    {
        _logger = logger;
    }

    public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(
        TRequest request,
        ServerCallContext context,
        UnaryServerMethod<TRequest, TResponse> continuation)
    {
        try
        {
            _logger.LogInformation($"Processing gRPC request: {context.Method}: {request}");

            return await continuation(request, context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"gRPC exception: {ex.Message}");

            throw new RpcException(new Status(StatusCode.InvalidArgument, ex.Message));
        }
    }
}
