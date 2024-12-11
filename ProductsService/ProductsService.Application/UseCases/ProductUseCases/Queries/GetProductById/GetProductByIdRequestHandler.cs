using AutoMapper;
using MediatR;
using ProductsService.Application.DTO;
using ProductsService.Application.Exceptions;
using ProductsService.Domain.Abstractions.Database;
using ProductsService.Domain.Abstractions.Specifications;
using ProductsService.Domain.Entities;

namespace ProductsService.Application.UseCases.ProductUseCases.Queries.GetProductById;

internal class GetProductByIdRequestHandler(IUnitOfWork unitOfWork, IMapper mapper, ISpecificationFactory specificationFactory)
    : IRequestHandler<GetProductByIdRequest, ResponseProductDTO>
{
    public async Task<ResponseProductDTO> Handle(GetProductByIdRequest request, CancellationToken cancellationToken)
    {
        var specification = specificationFactory.CreateSpecification<Product>();
        specification.Includes.Add(product => product.Categories);
        specification.Includes.Add(product => product.Discount);

        var product = await unitOfWork.ProductQueryRepository.GetByIdAsync(request.ProductId, specification, cancellationToken);

        if (product is null)
        {
            throw new NotFoundException("No such product");
        }

        var productDto = mapper.Map<ResponseProductDTO>(product);

        return productDto;
    }
}
