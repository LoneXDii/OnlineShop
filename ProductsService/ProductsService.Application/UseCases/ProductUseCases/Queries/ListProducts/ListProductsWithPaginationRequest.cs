using MediatR;
using ProductsService.Application.DTO;
using ProductsService.Domain.Common.Models;

namespace ProductsService.Application.UseCases.ProductUseCases.Queries.ListProducts;

public sealed record ListProductsWithPaginationRequest(ListProductsRequestDTO requestDto, Dictionary<string, string>? attributes)
    : IRequest<PaginatedListModel<ProductDTO>>
{ }