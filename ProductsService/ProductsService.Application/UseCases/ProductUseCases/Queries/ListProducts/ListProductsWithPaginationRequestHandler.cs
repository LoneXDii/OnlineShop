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

namespace ProductsService.Application.UseCases.ProductUseCases.Queries.ListProducts;

internal class ListProductsWithPaginationRequestHandler(IUnitOfWork unitOfWork, IMapper mapper, IOptions<PaginationSettings> paginationOptions)
    : IRequestHandler<ListProductsWithPaginationRequest, PaginatedListModel<ProductDTO>>
{
    public async Task<PaginatedListModel<ProductDTO>> Handle(ListProductsWithPaginationRequest request, CancellationToken cancellationToken)
    {
        var specification = new CombinableSpecification<Product>();
        //specification = specification & new ProductIncludesSpecification();

        //if (request.requestDto.CategoryId is not null)
        //{
        //    specification = specification & new ProductCategorySpecification(request.requestDto.CategoryId.Value);
        //}

        //if (request.requestDto.MinPrice is not null)
        //{
        //    specification = specification & new ProductMinPriceSpecification(request.requestDto.MinPrice.Value);
        //}

        //if (request.requestDto.MaxPrice is not null)
        //{
        //    specification = specification & new ProductMaxPriceSpecification(request.requestDto.MaxPrice.Value);
        //}

        //if (request.attributes is not null)
        //{
        //    foreach (var attribute in request.attributes)
        //    {
        //        specification = specification & new ProductAttributeValueSpecification(attribute.Key, attribute.Value);
        //    }
        //}

        var maxPageSize = paginationOptions.Value.MaxPageSize;

        var pageSize = request.requestDto.PageSize > maxPageSize
            ? maxPageSize
            : request.requestDto.PageSize;

        var items = await unitOfWork.ProductQueryRepository.ListWithPaginationAsync(request.requestDto.PageNo, pageSize, 
            specification, cancellationToken);

        var totalPages = (int)Math.Ceiling(items.Count() / (double)pageSize);

        if (request.requestDto.PageNo > totalPages)
        {
            throw new NotFoundException("No such page");
        }

        var data = new PaginatedListModel<ProductDTO>
        {
            Items = mapper.Map<List<ProductDTO>>(items),
            CurrentPage = request.requestDto.PageNo,
            TotalPages = totalPages
        };

        return data;
    }
}
