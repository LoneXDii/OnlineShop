using Grpc.Core;
using Grpc.Core.Interceptors;
using Microsoft.AspNetCore.Http;
using System.Net.Http;

namespace OrderService.Infrastructure.Interceptors;

internal class AuthInterceptor : Interceptor
{
    private readonly HttpContext _httpContext;

    public AuthInterceptor(IHttpContextAccessor httpContextAccessor)
    {
        _httpContext = httpContextAccessor.HttpContext;
    }

    public override AsyncUnaryCall<TResponse> AsyncUnaryCall<TRequest, TResponse>(TRequest request, ClientInterceptorContext<TRequest, TResponse> context, AsyncUnaryCallContinuation<TRequest, TResponse> continuation)
    {
        var header = _httpContext.Request.Headers["Authorization"].ToString();

        var metadata = new Metadata
        {
            {"Authorization", header}
        };

        var callOption = context.Options.WithHeaders(metadata);
        context = new ClientInterceptorContext<TRequest, TResponse>(context.Method, context.Host, callOption);

        return base.AsyncUnaryCall(request, context, continuation);
    }
}
