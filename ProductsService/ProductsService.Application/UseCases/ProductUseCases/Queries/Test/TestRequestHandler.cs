using AutoMapper;
using MediatR;
using ProductsService.Application.DTO;
using ProductsService.Application.Specifications;
using ProductsService.Application.Specifications.Products;
using ProductsService.Domain.Abstractions.Database;
using ProductsService.Domain.Entities;
using System.Diagnostics;
using System.Linq.Expressions;

namespace ProductsService.Application.UseCases.ProductUseCases.Queries.Test;

internal class TestRequestHandler(IUnitOfWork unitOfWork, IMapper mapper) 
    : IRequestHandler<TestRequest, IEnumerable<ProductDTO>>
{
    public async Task<IEnumerable<ProductDTO>> Handle(TestRequest request, CancellationToken cancellationToken) 
    {
        var specification = (CombinableSpecification<Product>)new EmptyProductSpecification();
        specification = specification & new ProductCategorySpecification(request.categoryId);
        //specification = specification & new ProductAttributeValueSpecification("Brand", "Apple");
        //specification = specification & new ProductAttributeValueSpecification("RAM", "16GB");
        specification = specification & new ProductMaxPriceSpecification(request.maxPrice);

        var products = await unitOfWork.ProductQueryRepository.ListAsync(specification);

        var ret = mapper.Map<List<ProductDTO>>(products);

        return ret;
    }
}
