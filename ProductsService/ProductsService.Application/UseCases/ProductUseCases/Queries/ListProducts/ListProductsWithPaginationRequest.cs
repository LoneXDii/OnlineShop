using MediatR;
using ProductsService.Application.DTO;
using ProductsService.Application.Models;

namespace ProductsService.Application.UseCases.ProductUseCases.Queries.ListProducts;

public sealed record ListProductsWithPaginationRequest(
    double? MaxPrice,
    double? MinPrice,
    int? CategoryId,
    int PageNo = 1, 
    int PageSize = 10, 
    params int[]? ValuesIds)
    : IRequest<PaginatedListModel<ProductDTO>>
{ }