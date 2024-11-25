using AutoMapper;
using MediatR;
using ProductsService.Domain.Abstractions.BlobStorage;
using ProductsService.Domain.Abstractions.Database;

namespace ProductsService.Application.UseCases.ProductUseCases.Commands.UpdateProduct;

internal class UpdateProductRequestHandler(IUnitOfWork unitOfWork, IMapper mapper, IBlobService blobService)
	: IRequestHandler<UpdateProductRequest>
{
	public async Task Handle(UpdateProductRequest request, CancellationToken cancellationToken)
	{
		var product = await unitOfWork.ProductQueryRepository.GetByIdAsync(request.productDTO.Id);

		mapper.Map(request.productDTO, product);

		if (request.productDTO.Image is not null)
		{
			if (product.ImageUrl is not null)
			{
				await blobService.DeleteAsync(product.ImageUrl);
			}

			using Stream stream = request.productDTO.Image.OpenReadStream();

			product.ImageUrl = await blobService.UploadAsync(stream, request.productDTO.Image.ContentType);
		}

		await unitOfWork.ProductCommandRepository.UpdateAsync(product, cancellationToken);

		await unitOfWork.SaveAllAsync(cancellationToken);
	}
}
