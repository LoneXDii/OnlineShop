using AutoMapper;
using MediatR;
using ProductsService.Application.Exceptions;
using ProductsService.Domain.Abstractions.BlobStorage;
using ProductsService.Domain.Abstractions.Database;
using ProductsService.Domain.Entities;

namespace ProductsService.Application.UseCases.CategoryUseCases.Commands.UpdateCategory;

internal class UpdateCategoryRequestHandler(IUnitOfWork unitOfWork, IBlobService blobService, IMapper mapper)
    : IRequestHandler<UpdateCategoryRequest>
{
    public async Task Handle(UpdateCategoryRequest request, CancellationToken cancellationToken)
    {
        var category = await unitOfWork.CategoryQueryRepository.GetByIdAsync(request.CategoryId, cancellationToken);

        if (category is null)
        {
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
    }
}
