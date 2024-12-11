using AutoMapper;
using MediatR;
using ProductsService.Application.DTO;
using ProductsService.Application.Exceptions;
using ProductsService.Domain.Abstractions.Database;
using ProductsService.Application.Models;
using ProductsService.Domain.Entities;
using Microsoft.Extensions.Options;
using ProductsService.Application.Configuration;
using ProductsService.Domain.Abstractions.Specifications;

namespace ProductsService.Application.UseCases.ProductUseCases.Queries.ListProducts;

internal class ListProductsWithPaginationRequestHandler(IUnitOfWork unitOfWork, IMapper mapper, 
    IOptions<PaginationSettings> paginationOptions, ISpecificationFactory specificationFactory)
    : IRequestHandler<ListProductsWithPaginationRequest, PaginatedListModel<ResponseProductDTO>>
{
    public async Task<PaginatedListModel<ResponseProductDTO>> Handle(ListProductsWithPaginationRequest request, CancellationToken cancellationToken)
    {
        var specification = specificationFactory.CreateSpecification<Product>();
        specification.Includes.Add(product => product.Categories);
        specification.Includes.Add(product => product.Discount);

        if (request.CategoryId is not null)
        {
            specification.Criteries.Add(product => product.Categories.Any(c => c.Id == request.CategoryId));
        }

        if (request.MinPrice is not null)
        {
            specification.Criteries.Add(product => product.Price >= request.MinPrice);
        }

        if (request.MaxPrice is not null)
        {
            specification.Criteries.Add(product => product.Price <= request.MaxPrice);
        }

        if (request.ValuesIds is not null)
        {
            foreach (var id in request.ValuesIds)
            {
                specification.Criteries.Add(product => product.Categories.Any(c => c.Id == id));
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

        var data = new PaginatedListModel<ResponseProductDTO>
        {
            Items = mapper.Map<List<ResponseProductDTO>>(items),
            CurrentPage = request.PageNo,
            TotalPages = totalPages
        };

        return data;
    }
}
