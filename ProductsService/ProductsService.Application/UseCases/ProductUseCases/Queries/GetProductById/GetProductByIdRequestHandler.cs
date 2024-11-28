using AutoMapper;
using MediatR;
using ProductsService.Application.DTO;
using ProductsService.Application.Exceptions;
using ProductsService.Domain.Abstractions.Database;

namespace ProductsService.Application.UseCases.ProductUseCases.Queries.GetProductById;

internal class GetProductByIdRequestHandler(IUnitOfWork unitOfWork, IMapper mapper)
    : IRequestHandler<GetProductByIdRequest, ProductDTO>
{
    public async Task<ProductDTO> Handle(GetProductByIdRequest request, CancellationToken cancellationToken)
    {
        var product = await unitOfWork.ProductQueryRepository.GetByIdAsync(request.ProductId, cancellationToken, p => p.Categories);

        if (product is null)
        {
            throw new NotFoundException("No such product");
        }

        var productDto = mapper.Map<ProductDTO>(product);

        return productDto;
    }
}
