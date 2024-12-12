using AutoMapper;
using MediatR;
using ProductsService.Application.DTO;
using ProductsService.Application.Exceptions;
using ProductsService.Domain.Abstractions.Database;

namespace ProductsService.Application.UseCases.ProductUseCases.Queries.GetProductById;

internal class GetProductByIdRequestHandler(IUnitOfWork unitOfWork, IMapper mapper)
    : IRequestHandler<GetProductByIdRequest, ResponseProductDTO>
{
    public async Task<ResponseProductDTO> Handle(GetProductByIdRequest request, CancellationToken cancellationToken)
    {
        var product = await unitOfWork.ProductQueryRepository.GetByIdAsync(request.ProductId, cancellationToken,
            product => product.Categories, product => product.Discount);

        if (product is null)
        {
            throw new NotFoundException("No such product");
        }

        return mapper.Map<ResponseProductDTO>(product);
    }
}
