using AutoMapper;
using MediatR;
using ProductsService.Application.Exceptions;
using ProductsService.Domain.Abstractions.Database;
using ProductsService.Domain.Abstractions.Specifications;
using ProductsService.Domain.Entities;

namespace ProductsService.Application.UseCases.DiscountUseCases.Commands.AddDiscount;

internal class AddDiscountRequestHandler(IUnitOfWork unitOfWork, IMapper mapper, ISpecificationFactory specificationFactory)
    : IRequestHandler<AddDiscountRequest>
{
    public async Task Handle(AddDiscountRequest request, CancellationToken cancellationToken)
    {
        var specification = specificationFactory.CreateSpecification<Discount>();
        specification.Criteries.Add(discount => discount.ProductId == request.ProductId);

        var discount = await unitOfWork.DiscountQueryRepository.FirstOrDefaultAsync(specification, cancellationToken);

        if(discount is not null)
        {
            throw new BadRequestException("This product already have discount");
        }

        discount = mapper.Map<Discount>(request);

        await unitOfWork.DiscountCommandRepository.AddAsync(discount, cancellationToken);

        await unitOfWork.SaveAllAsync(cancellationToken);
    }
}
