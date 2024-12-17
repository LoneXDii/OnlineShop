using AutoMapper;
using Grpc.Net.Client;
using Microsoft.Extensions.Options;
using OrderService.Domain.Abstractions.Data;
using OrderService.Domain.Entities;
using OrderService.Infrastructure.Configuration;
using OrderService.Infrastructure.Protos;

namespace OrderService.Infrastructure.Services;

internal class GrpcProductService : IProductService
{
    private readonly GrpcSettings _grpcSettings;
    private readonly IMapper _mapper;

    public GrpcProductService(IOptions<GrpcSettings> grpcSettings, IMapper mapper)
    {
        _grpcSettings = grpcSettings.Value;
        _mapper = mapper;
    }

    public async Task<ProductEntity?> GetByIdIfSufficientQuantityAsync(int id, int quantity, 
        CancellationToken cancellationToken = default)
    {
        using var channel = GrpcChannel.ForAddress(_grpcSettings.ServerUrl);
        
        var client = new Products.ProductsClient(channel);

        var request = new ProductRequest 
        { 
            Id = id, 
            Quantity = quantity 
        };

        var response = await client.GetProductAsync(request, null, null, cancellationToken);

        return _mapper.Map<ProductEntity>(response);
    }

    public Task<IEnumerable<ProductEntity>?> TakeProductsIfSufficientQuantityAsync(IEnumerable<ProductEntity> products, 
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task ReturnProductsAsync(IEnumerable<ProductEntity> products, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
