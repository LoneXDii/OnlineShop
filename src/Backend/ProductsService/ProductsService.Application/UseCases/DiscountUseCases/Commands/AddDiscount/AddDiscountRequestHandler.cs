using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using ProductsService.Application.Exceptions;
using ProductsService.Application.Proxy;
using ProductsService.Application.Specifications.Discounts;
using ProductsService.Domain.Abstractions.Database;
using ProductsService.Domain.Entities;

namespace ProductsService.Application.UseCases.DiscountUseCases.Commands.AddDiscount;

internal class AddDiscountRequestHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<AddDiscountRequestHandler> logger,
    IBackgroundJobProxy backgroundJob)
    : IRequestHandler<AddDiscountRequest>
{
    public async Task Handle(AddDiscountRequest request, CancellationToken cancellationToken)
    {
        logger.LogInformation($"Trying to create discount for product with id: {request.ProductId}");

        var specification = new DiscountProductSpecification(request.ProductId);

        var discount = await unitOfWork.DiscountQueryRepository.FirstOrDefaultAsync(specification, cancellationToken);

        if (discount is not null)
        {
            logger.LogError($"Product with id: {request.ProductId} already has a discount with id: {discount.Id}");

            throw new BadRequestException("This product already have discount");
        }

        discount = mapper.Map<Discount>(request);

        await unitOfWork.DiscountCommandRepository.AddAsync(discount, cancellationToken);

        await unitOfWork.SaveAllAsync(cancellationToken);

        backgroundJob.Schedule(() => unitOfWork.DiscountCommandRepository.DeleteAsync(discount, default), discount.EndDate);

        logger.LogInformation($"Discount with id: {discount.Id} succesfully created for product with id: {request.ProductId}");
    }
}
