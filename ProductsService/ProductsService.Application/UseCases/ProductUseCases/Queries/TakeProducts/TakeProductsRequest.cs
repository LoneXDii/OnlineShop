using MediatR;
using ProductsService.Domain.Entities;

namespace ProductsService.Application.UseCases.ProductUseCases.Queries.TakeProducts;

public sealed record TakeProductsRequest(IDictionary<int, int> Products) : IRequest<IEnumerable<Product>> { }
