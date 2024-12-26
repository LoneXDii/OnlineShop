using MediatR;

namespace ProductsService.Application.UseCases.ProductUseCases.Commands.ReturnProducts;

public sealed record ReturnProductsRequest(IDictionary<int, int> Products) : IRequest { }
