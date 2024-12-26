using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using ProductsService.Application.DTO;
using ProductsService.Application.Exceptions;
using ProductsService.Domain.Abstractions.Database;

namespace ProductsService.Application.UseCases.ProductUseCases.Queries.GetProductById;

internal class GetProductByIdRequestHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<GetProductByIdRequestHandler> logger)
    : IRequestHandler<GetProductByIdRequest, ResponseProductDTO>
{
    public async Task<ResponseProductDTO> Handle(GetProductByIdRequest request, CancellationToken cancellationToken)
    {
        logger.LogInformation($"Finding product with id: {request.ProductId}");

        var product = await unitOfWork.ProductQueryRepository.GetByIdAsync(request.ProductId, cancellationToken,
            product => product.Categories, product => product.Discount);

        if (product is null)
        {
            logger.LogError($"No product with id: {request.ProductId} found");

            throw new NotFoundException("No such product");
        }

        return mapper.Map<ResponseProductDTO>(product);
    }
}
