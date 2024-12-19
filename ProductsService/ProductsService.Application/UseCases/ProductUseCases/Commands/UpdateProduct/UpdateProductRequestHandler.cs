using AutoMapper;
using Hangfire;
using MediatR;
using ProductsService.Domain.Abstractions.BlobStorage;
using ProductsService.Domain.Abstractions.Database;
using ProductsService.Domain.Abstractions.MessageBrocker;

namespace ProductsService.Application.UseCases.ProductUseCases.Commands.UpdateProduct;

internal class UpdateProductRequestHandler(IUnitOfWork unitOfWork, IMapper mapper, IBlobService blobService,
    IProducerService producerService)
    : IRequestHandler<UpdateProductRequest>
{
    public async Task Handle(UpdateProductRequest request, CancellationToken cancellationToken)
    {
        var product = await unitOfWork.ProductQueryRepository.GetByIdAsync(request.Id);

        var oldPrice = product.Price;

        mapper.Map(request, product);

        if (request.Image is not null)
        {
            if (product.ImageUrl is not null)
            {
                await blobService.DeleteAsync(product.ImageUrl);
            }

            product.ImageUrl = await blobService.UploadAsync(request.Image, request.ImageContentType);

            request.Image.Dispose();
        }

        await unitOfWork.ProductCommandRepository.UpdateAsync(product, cancellationToken);

        if (oldPrice != product.Price)
        {
            await producerService.ProduceProductCreationAsync(product, default);
        }

        await unitOfWork.SaveAllAsync(cancellationToken);
    }
}
