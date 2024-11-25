using AutoMapper;
using MediatR;
using ProductsService.Domain.Abstractions.Database;
using ProductsService.Domain.Entities;

namespace ProductsService.Application.UseCases.ProductUseCases.Commands.AddAttributeToProduct;

internal class AddAttributeToProductRequestHandler(IUnitOfWork unitOfWork, IMapper mapper) 
    : IRequestHandler<AddAttributeToProductRequest>
{
    public async Task Handle(AddAttributeToProductRequest request, CancellationToken cancellationToken)
    {
        var productAttribute = mapper.Map<ProductAttribute>(request.attributeValue);

        await unitOfWork.ProductAttributeCommandRepository.AddAsync(productAttribute, cancellationToken);

        await unitOfWork.SaveAllAsync(cancellationToken);
    }
}
