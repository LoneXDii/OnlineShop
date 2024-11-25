using AutoMapper;
using MediatR;
using ProductsService.Application.Exceptions;
using ProductsService.Domain.Abstractions.Database;
using ProductsService.Domain.Entities;

namespace ProductsService.Application.UseCases.ProductUseCases.Commands.AddAttributeToProduct;

internal class AddAttributeToProductRequestHandler(IUnitOfWork unitOfWork, IMapper mapper) 
    : IRequestHandler<AddAttributeToProductRequest>
{
    public async Task Handle(AddAttributeToProductRequest request, CancellationToken cancellationToken)
    {
        var productAttribute = mapper.Map<ProductAttribute>(request.attributeValue);

        var product = await unitOfWork.ProductQueryRepository.GetByIdAsync(productAttribute.ProductId, cancellationToken);

        var attribute = await unitOfWork.AttributeQueryRepository.GetByIdAsync(productAttribute.AttributeId, cancellationToken);

        if(product is null || attribute is null)
        {
            throw new BadRequestException("No such product or attribute");
        }

        var productAttributeDb = await unitOfWork.ProductAttributeQueryRepository.FirstOrDefaultAsync(pa 
            => pa.ProductId == productAttribute.ProductId 
            && pa.AttributeId == productAttribute.AttributeId);

        if (productAttributeDb is not null) 
        {
            throw new BadRequestException("This attribute is already exists for this product");
        }

        await unitOfWork.ProductAttributeCommandRepository.AddAsync(productAttribute, cancellationToken);

        await unitOfWork.SaveAllAsync(cancellationToken);
    }
}
