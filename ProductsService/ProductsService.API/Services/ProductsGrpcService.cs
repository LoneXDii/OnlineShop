using AutoMapper;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using ProductsService.API.Protos;
using ProductsService.Application.UseCases.ProductUseCases.Queries.GetProductIfSufficientQuantity;
using ProductsService.Application.UseCases.ProductUseCases.Queries.ReturnProducts;
using ProductsService.Application.UseCases.ProductUseCases.Queries.TakeProducts;

namespace ProductsService.API.Services;

public class ProductsGrpcService : Products.ProductsBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public ProductsGrpcService(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }
    
    public override async Task<ProductResponse> GetProduct(ProductRequest request, ServerCallContext context)
    {
        var product = await _mediator.Send(new GetProductIfSufficientQuantityRequest(request.Id, request.Quantity));

        return _mapper.Map<ProductResponse>(product);
    }

    [Authorize]
    public override async Task<ProductsListResponse> TakeProducts(ProductsListRequest request, ServerCallContext context)
    {
        var dict = request.Products.ToDictionary(pr => pr.Id, pr => pr.Quantity);

        var data = await _mediator.Send(new TakeProductsRequest(dict));

        var products = _mapper.Map<List<ProductResponse>>(data);

        var response = new ProductsListResponse();
        response.Products.AddRange(products);

        return response;
    }

    [Authorize]
    public override async Task<Empty> ReturnProducts(ProductsListRequest request, ServerCallContext context)
    {
        var products = request.Products.ToDictionary(pr => pr.Id, pr => pr.Quantity);

        await _mediator.Send(new ReturnProductsRequest(products));

        return new Empty();
    }
}
