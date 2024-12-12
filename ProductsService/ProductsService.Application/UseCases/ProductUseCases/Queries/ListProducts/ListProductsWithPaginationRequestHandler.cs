using AutoMapper;
using MediatR;
using ProductsService.Application.DTO;
using ProductsService.Application.Exceptions;
using ProductsService.Domain.Abstractions.Database;
using ProductsService.Application.Models;
using Microsoft.Extensions.Options;
using ProductsService.Application.Configuration;
using ProductsService.Application.Specifications.Products;

namespace ProductsService.Application.UseCases.ProductUseCases.Queries.ListProducts;

internal class ListProductsWithPaginationRequestHandler(IUnitOfWork unitOfWork, IMapper mapper, 
    IOptions<PaginationSettings> paginationOptions)
    : IRequestHandler<ListProductsWithPaginationRequest, PaginatedListModel<ResponseProductDTO>>
{
    public async Task<PaginatedListModel<ResponseProductDTO>> Handle(ListProductsWithPaginationRequest request, CancellationToken cancellationToken)
    {
        var categorySpecification = new ProductCategorySpecification(request.CategoryId);

        var specification = categorySpecification
            .And(new ProductMinPriceSpecification(request.MinPrice))
            .And(new ProductMaxPriceSpecification(request.MaxPrice))
            .And(new ProductAttributesSpecification(request.ValuesIds));

        var maxPageSize = paginationOptions.Value.MaxPageSize;

        var pageSize = request.PageSize > maxPageSize
            ? maxPageSize
            : request.PageSize;

        var items = await unitOfWork.ProductQueryRepository.ListWithPaginationAsync(request.PageNo, pageSize,
            specification, cancellationToken, 
            product => product.Categories,
            product => product.Discount);

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
