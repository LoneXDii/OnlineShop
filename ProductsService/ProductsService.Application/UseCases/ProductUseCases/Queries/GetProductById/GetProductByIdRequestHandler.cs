using AutoMapper;
using MediatR;
using ProductsService.Application.DTO;
using ProductsService.Application.Exceptions;
using ProductsService.Application.Specifications;
using ProductsService.Application.Specifications.Products;
using ProductsService.Domain.Abstractions.Database;
using ProductsService.Domain.Entities;

namespace ProductsService.Application.UseCases.ProductUseCases.Queries.GetProductById;

internal class GetProductByIdRequestHandler(IUnitOfWork unitOfWork, IMapper mapper)
    : IRequestHandler<GetProductByIdRequest, ProductDTO>
{
    public async Task<ProductDTO> Handle(GetProductByIdRequest request, CancellationToken cancellationToken)
    {
        var specification = new CombinableSpecification<Product>();
        specification = specification & new ProductIncludeCategoriesSpecification();
        specification = specification & new ProductIncludeDiscountSpecification();

        var product = await unitOfWork.ProductQueryRepository.GetByIdAsync(request.ProductId, specification, cancellationToken);

        if (product is null)
        {
            throw new NotFoundException("No such product");
        }

        var productDto = mapper.Map<ProductDTO>(product);

        return productDto;
    }
}
