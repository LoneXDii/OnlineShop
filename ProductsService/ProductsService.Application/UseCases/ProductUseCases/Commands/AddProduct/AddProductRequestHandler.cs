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
        var product = mapper.Map<Product>(request.product);

        if(request.product.Image is not null)
        {
            using var stream = request.product.Image.OpenReadStream();

            product.ImageUrl = await blobService.UploadAsync(stream, request.product.Image.ContentType);
        }

        await unitOfWork.ProductCommandRepository.AddAsync(product);

        await unitOfWork.SaveAllAsync();
    }
}
