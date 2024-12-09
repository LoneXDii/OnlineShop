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
            using var stream = request.Image.OpenReadStream();

            category.ImageUrl = await blobService.UploadAsync(stream, request.Image.ContentType);
        }

        await unitOfWork.CategoryCommandRepository.AddAsync(category, cancellationToken);

        await unitOfWork.SaveAllAsync(cancellationToken);
    }
}
