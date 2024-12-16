using Grpc.Core;
using ProductsService.API.Protos;

namespace ProductsService.API.Services;

public class ProductsGrpcService : Products.ProductsBase
{
    public override async Task<GetProductResponse> GetProduct(GetProductRequest request, ServerCallContext context)
    {
        return await Task.FromResult(new GetProductResponse
        {
            Message = $"Product: id={request.Id}, quantity={request.Quantity}"
        });
    }
}
