using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using ProductsService.Application.Exceptions;
using ProductsService.Domain.Abstractions.BlobStorage;
using ProductsService.Domain.Abstractions.Database;
using ProductsService.Domain.Entities;

namespace ProductsService.Application.UseCases.CategoryUseCases.Commands.UpdateCategory;

internal class UpdateCategoryRequestHandler(IUnitOfWork unitOfWork, IBlobService blobService, IMapper mapper,
    ILogger<UpdateCategoryRequestHandler> logger)
    : IRequestHandler<UpdateCategoryRequest>
{
    public async Task Handle(UpdateCategoryRequest request, CancellationToken cancellationToken)
    {
        logger.LogInformation($"Trying to update category with id: {request.CategoryId}");

        var category = await unitOfWork.CategoryQueryRepository.GetByIdAsync(request.CategoryId, cancellationToken);

        if (category is null)
        {
            logger.LogError($"Category with id: {request.CategoryId} not found");

            throw new BadRequestException("No such category");
        }

        mapper.Map(request, category);

        if (request.Image is not null)
        {
            if (category.ImageUrl is not null)
            {
                await blobService.DeleteAsync(category.ImageUrl);
            }

            category.ImageUrl = await blobService.UploadAsync(request.Image, request.ImageContentType);

            request.Image.Dispose();
        }

        await unitOfWork.CategoryCommandRepository.UpdateAsync(category, cancellationToken);

        await unitOfWork.SaveAllAsync(cancellationToken);

        logger.LogInformation($"Category with id: {category.Id} successfully updated");
    }
}
