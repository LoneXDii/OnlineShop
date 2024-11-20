using MediatR;
using ProductsService.Application.DTO;
using ProductsService.Domain.Entities;

namespace ProductsService.Application.UseCases.ProductUseCases.Queries.Test;

public sealed record TestRequest(int categoryId, double maxPrice) : IRequest<IEnumerable<ProductDTO>> { }