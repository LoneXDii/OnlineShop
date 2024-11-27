using AutoMapper;
using MediatR;
using ProductsService.Application.DTO;
using ProductsService.Application.Exceptions;
using ProductsService.Application.Specifications;
using ProductsService.Domain.Abstractions.Database;
using ProductsService.Application.Models;
using ProductsService.Domain.Entities;
using Microsoft.Extensions.Options;
using ProductsService.Application.Configuration;
using ProductsService.Application.Specifications.Products;

namespace ProductsService.Application.UseCases.ProductUseCases.Queries.ListProducts;

internal class ListProductsWithPaginationRequestHandler(IUnitOfWork unitOfWork, IMapper mapper, IOptions<PaginationSettings> paginationOptions)
    : IRequestHandler<ListProductsWithPaginationRequest, PaginatedListModel<ProductDTO>>
{
    public async Task<PaginatedListModel<ProductDTO>> Handle(ListProductsWithPaginationRequest request, CancellationToken cancellationToken)
    {
        var specification = new CombinableSpecification<Product>();
        specification = specification & new ProductIncludesSpecification();

        if (request.CategoryId is not null)
        {
            specification = specification & new ProductCategorySpecification(request.CategoryId.Value);
        }

        if (request.MinPrice is not null)
        {
            specification = specification & new ProductMinPriceSpecification(request.MinPrice.Value);
        }

        if (request.MaxPrice is not null)
        {
            specification = specification & new ProductMaxPriceSpecification(request.MaxPrice.Value);
        }

        if (request.ValuesIds is not null)
        {
            foreach (var id in request.ValuesIds)
            {
                specification = specification & new ProductCategorySpecification(id);
            }
        }

        var maxPageSize = paginationOptions.Value.MaxPageSize;

        var pageSize = request.PageSize > maxPageSize
            ? maxPageSize
            : request.PageSize;

        var items = await unitOfWork.ProductQueryRepository.ListWithPaginationAsync(request.PageNo, pageSize,
            specification, cancellationToken);

        var totalPages = (int)Math.Ceiling(items.Count() / (double)pageSize);

        if (request.PageNo > totalPages)
        {
            throw new NotFoundException("No such page");
        }

        var data = new PaginatedListModel<ProductDTO>
        {
            Items = mapper.Map<List<ProductDTO>>(items),
            CurrentPage = request.PageNo,
            TotalPages = totalPages
        };

        return data;
    }
}
