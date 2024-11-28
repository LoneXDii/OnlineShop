using AutoMapper;
using MediatR;
using ProductsService.Domain.Abstractions.BlobStorage;
using ProductsService.Domain.Abstractions.Database;
using ProductsService.Domain.Entities;

namespace ProductsService.Application.UseCases.ProductUseCases.Commands.AddProduct;

internal class AddProductRequestHandler(IUnitOfWork unitOfWork, IBlobService blobService, IMapper mapper)
    : IRequestHandler<AddProductRequest>
{
    public async Task Handle(AddProductRequest request, CancellationToken cancellationToken)
    {
        var product = mapper.Map<Product>(request);

        if(request.Image is not null)
        {
            using var stream = request.Image.OpenReadStream();

            product.ImageUrl = await blobService.UploadAsync(stream, request.Image.ContentType);
        }

        foreach (var category in product.Categories)
        {
            unitOfWork.AttachInCommandContext(category);
        }

        await unitOfWork.ProductCommandRepository.AddAsync(product, cancellationToken);

        await unitOfWork.SaveAllAsync(cancellationToken);
    }
}
