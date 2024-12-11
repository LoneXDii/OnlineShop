using AutoMapper;
using MediatR;
using ProductsService.Domain.Abstractions.BlobStorage;
using ProductsService.Domain.Abstractions.Database;
using ProductsService.Domain.Entities;

namespace ProductsService.Application.UseCases.CategoryUseCases.Commands.AddCategory;

internal class AddCategoryRequestHandler(IUnitOfWork unitOfWork, IBlobService blobService, IMapper mapper)
    : IRequestHandler<AddCategoryRequest>
{
    public async Task Handle(AddCategoryRequest request, CancellationToken cancellationToken)
    {
        var category = mapper.Map<Category>(request);

        if (request.Image is not null)
        {
            category.ImageUrl = await blobService.UploadAsync(request.Image, request.ImageContentType);

            request.Image.Dispose();
        }

        await unitOfWork.CategoryCommandRepository.AddAsync(category, cancellationToken);

        await unitOfWork.SaveAllAsync(cancellationToken);
    }
}
