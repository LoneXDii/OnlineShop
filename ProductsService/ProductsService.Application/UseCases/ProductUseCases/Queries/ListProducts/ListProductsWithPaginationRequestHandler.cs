using AutoMapper;
using MediatR;
using ProductsService.Application.DTO;
using ProductsService.Application.Specifications;
using ProductsService.Application.Specifications.Products;
using ProductsService.Domain.Abstractions.Database;
using ProductsService.Domain.Common.Models;
using ProductsService.Domain.Entities;

namespace ProductsService.Application.UseCases.ProductUseCases.Queries.ListProducts;

internal class ListProductsWithPaginationRequestHandler(IUnitOfWork unitOfWork, IMapper mapper)
    : IRequestHandler<ListProductsWithPaginationRequest, PaginatedListModel<ProductDTO>>
{
    public async Task<PaginatedListModel<ProductDTO>> Handle(ListProductsWithPaginationRequest request, CancellationToken cancellationToken)
    {
        //var specification = new CombinableSpecification<Product>();

        //Maybe add typeof to operators overloads;
        var specification = (CombinableSpecification<Product>)new EmptyProductSpecification();

        if (request.requestDto.CategoryId is not null)
        {
            specification = specification & new ProductCategorySpecification(request.requestDto.CategoryId.Value);
        }

        if(request.requestDto.MinPrice is not null)
        {
            specification = specification & new ProductMinPriceSpecification(request.requestDto.MinPrice.Value);
        }

        if (request.requestDto.MaxPrice is not null)
        {
            specification = specification & new ProductMaxPriceSpecification(request.requestDto.MaxPrice.Value);
        }

        if(request.attributes is not null)
        {
            foreach(var attribute in request.attributes)
            {
                specification = specification & new ProductAttributeValueSpecification(attribute.Key, attribute.Value);
            }
        }

        var data = await unitOfWork.ProductQueryRepository.ListWithPaginationAsync(request.requestDto.PageNo,
            request.requestDto.PageSize, specification);

        var retData = mapper.Map<PaginatedListModel<ProductDTO>>(data);
        return retData;
    }
}
