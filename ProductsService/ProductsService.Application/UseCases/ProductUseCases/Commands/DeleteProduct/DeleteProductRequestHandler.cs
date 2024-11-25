using MediatR;
using ProductsService.Application.Exceptions;
using ProductsService.Domain.Abstractions.BlobStorage;
using ProductsService.Domain.Abstractions.Database;

namespace ProductsService.Application.UseCases.ProductUseCases.Commands.DeleteProduct;

internal class DeleteProductRequestHandler(IUnitOfWork unitOfWork, IBlobService blobService)
    : IRequestHandler<DeleteProductRequest>
{
    public async Task Handle(DeleteProductRequest request, CancellationToken cancellationToken)
    {
        var product = await unitOfWork.ProductQueryRepository.GetByIdAsync(request.productId, cancellationToken);

        if(product is null)
        {
            throw new NotFoundException("No such product");
        }

        if(product.ImageUrl is not null)
        {
            await blobService.DeleteAsync(product.ImageUrl);
        }

        await unitOfWork.ProductCommandRepository.DeleteAsync(product, cancellationToken);

        await unitOfWork.SaveAllAsync(cancellationToken);
    }
}
