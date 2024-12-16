using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using MediatR;
using ProductsService.API.Protos;
using ProductsService.Application.UseCases.ProductUseCases.Queries.GetProductIfSufficientQuantity;
using ProductsService.Application.UseCases.ProductUseCases.Queries.ReturnProducts;
using ProductsService.Application.UseCases.ProductUseCases.Queries.TakeProducts;

namespace ProductsService.API.Services;
//TODO
//Add Auth, Validation, Mapping and Exceptions handling
public class ProductsGrpcService : Products.ProductsBase
{
    private readonly IMediator _mediator;

    public ProductsGrpcService(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    public override async Task<ProductResponse> GetProduct(ProductRequest request, ServerCallContext context)
    {
        var product = await _mediator.Send(new GetProductIfSufficientQuantityRequest(request.Id, request.Quantity));

        var response = new ProductResponse { 
            Id = product.Id,
            Name = product.Name,
            Price = product.Price,
            Quantity = product.Quantity,
            ImageUrl = product.ImageUrl ?? "",
            PriceId = product.PriceId ?? ""
        };

        return response;
    }

    public override async Task<ProductsListResponse> TakeProducts(ProductsListRequest request, ServerCallContext context)
    {
        var dict = request.Products.ToDictionary(pr => pr.Id, pr => pr.Quantity);

        var data = await _mediator.Send(new TakeProductsRequest(dict));

        var products = new List<ProductResponse>();

        foreach (var item in data)
        {
            products.Add(new ProductResponse
            {
                Id = item.Id,
                Name = item.Name,
                Price = item.Price,
                Quantity = item.Quantity,
                ImageUrl = item.ImageUrl ?? "",
                PriceId = item.PriceId ?? ""
            });
        }

        var response = new ProductsListResponse();
        response.Products.AddRange(products);

        return response;
    }

    public override async Task<Empty> ReturnProducts(ProductsListRequest request, ServerCallContext context)
    {
        var products = request.Products.ToDictionary(pr => pr.Id, pr => pr.Quantity);

        await _mediator.Send(new ReturnProductsRequest(products));

        return new Empty();
    }
}
