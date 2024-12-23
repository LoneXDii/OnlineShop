using MediatR;

namespace ProductsService.Application.UseCases.ProductUseCases.Queries.ReturnProducts;

public sealed record ReturnProductsRequest(IDictionary<int, int> Products) : IRequest { }
