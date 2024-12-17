using AutoMapper;
using Grpc.Net.Client;
using Grpc.Net.ClientFactory;
using Microsoft.Extensions.Options;
using OrderService.Domain.Abstractions.Data;
using OrderService.Domain.Entities;
using OrderService.Infrastructure.Protos;

namespace OrderService.Infrastructure.Services;

internal class GrpcProductService : IProductService
{
    private readonly Products.ProductsClient _gprcClient;
    private readonly IMapper _mapper;

    public GrpcProductService(Products.ProductsClient grpcClient, IMapper mapper)
    {
        _gprcClient = grpcClient;
        _mapper = mapper;
    }

    public async Task<ProductEntity?> GetByIdIfSufficientQuantityAsync(int id, int quantity, 
        CancellationToken cancellationToken = default)
    {
        var request = new ProductRequest 
        { 
            Id = id, 
            Quantity = quantity 
        };

        var response = await _gprcClient.GetProductAsync(request, null, null, cancellationToken);

        return _mapper.Map<ProductEntity>(response);
    }

    public async Task<IEnumerable<ProductEntity>?> TakeProductsIfSufficientQuantityAsync(IEnumerable<ProductEntity> products, 
        CancellationToken cancellationToken = default)
    {
        var request = new ProductsListRequest();
        request.Products.AddRange(_mapper.Map<List<ProductRequest>>(products));

        var resposne = await _gprcClient.TakeProductsAsync(request, null, null, cancellationToken);

        return _mapper.Map<List<ProductEntity>>(resposne.Products);
    }

    public async Task ReturnProductsAsync(IEnumerable<ProductEntity> products, CancellationToken cancellationToken = default)
    {
        var request = new ProductsListRequest();
        request.Products.AddRange(_mapper.Map<List<ProductRequest>>(products));

        await _gprcClient.ReturnProductsAsync(request, null, null, cancellationToken);
    }
}
