using AutoMapper;
using Microsoft.Extensions.Logging;
using OrderService.Domain.Abstractions.Data;
using OrderService.Domain.Entities;
using OrderService.Infrastructure.Protos;

namespace OrderService.Infrastructure.Services;

internal class GrpcProductService : IProductService
{
    private readonly Products.ProductsClient _gprcClient;
    private readonly IMapper _mapper;
    private readonly ILogger<GrpcProductService> _logger;

    public GrpcProductService(Products.ProductsClient grpcClient, IMapper mapper, ILogger<GrpcProductService> logger)
    {
        _gprcClient = grpcClient;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ProductEntity?> GetByIdIfSufficientQuantityAsync(int id, int quantity, 
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation($"Trying to get product with id: {id} and quantity: {quantity} using gRPC");

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
        _logger.LogInformation("Trying to take products using gRPC");

        var request = new ProductsListRequest();
        request.Products.AddRange(_mapper.Map<List<ProductRequest>>(products));

        var response = await _gprcClient.TakeProductsAsync(request, null, null, cancellationToken);

        return _mapper.Map<List<ProductEntity>>(response.Products);
    }

    public async Task ReturnProductsAsync(IEnumerable<ProductEntity> products, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Trying to return products using gRPC");

        var request = new ProductsListRequest();
        var a = _mapper.Map<List<ProductRequest>>(products);
        request.Products.AddRange(_mapper.Map<List<ProductRequest>>(products));

        await _gprcClient.ReturnProductsAsync(request, null, null, cancellationToken);
    }
}
