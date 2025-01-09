using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using ProductsService.Application.Exceptions;
using ProductsService.Application.Specifications.Categories;
using ProductsService.Domain.Abstractions.BlobStorage;
using ProductsService.Domain.Abstractions.Database;
using ProductsService.Domain.Abstractions.MessageBrocker;

namespace ProductsService.Application.UseCases.ProductUseCases.Commands.UpdateProduct;

internal class UpdateProductRequestHandler(IUnitOfWork unitOfWork, IMapper mapper, IBlobService blobService,
    IProducerService producerService, ILogger<UpdateProductRequestHandler> logger)
    : IRequestHandler<UpdateProductRequest>
{
    public async Task Handle(UpdateProductRequest request, CancellationToken cancellationToken)
    {
        logger.LogInformation($"Trying to update product with id: {request.Id}");

        var product = await unitOfWork.ProductQueryRepository.GetByIdAsync(request.Id, cancellationToken, p => p.Categories);

        if (product is null)
        {
            logger.LogError($"No product with id: {request.Id} found");

            throw new BadRequestException("No such product");
        }

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

        if (request.Attributes is not null)
        {
            var specification = new CategoriesByIdsArraySpecification(request.Attributes);

            var categories = await unitOfWork.CategoryQueryRepository.ListAsync(specification, cancellationToken);

            unitOfWork.AttachInCommandContext(product);
            unitOfWork.AttachInCommandContext(categories);

            product.Categories.Clear();

            foreach (var category in categories)
            {
                product.Categories.Add(category);
            }
        }

        await unitOfWork.ProductCommandRepository.UpdateAsync(product, cancellationToken);

        if (oldPrice != product.Price)
        {
            await producerService.ProduceProductCreationAsync(product, default);
        }

        await unitOfWork.SaveAllAsync(cancellationToken);

        logger.LogInformation($"Product with id: {product.Id} succesfully updated");
    }
}
