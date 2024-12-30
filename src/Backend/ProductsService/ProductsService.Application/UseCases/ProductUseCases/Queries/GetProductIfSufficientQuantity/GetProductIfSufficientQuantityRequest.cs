using MediatR;
using ProductsService.Domain.Entities;

namespace ProductsService.Application.UseCases.ProductUseCases.Queries.GetProductIfSufficientQuantity;

public sealed record GetProductIfSufficientQuantityRequest(int Id, int Quantity) : IRequest<Product> { }
