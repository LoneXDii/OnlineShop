using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using ProductsService.Application.Exceptions;
using ProductsService.Domain.Abstractions.Database;
using ProductsService.Domain.Entities;

namespace ProductsService.Application.UseCases.CategoryUseCases.Commands.AddAttribute;

internal class AddAttributeRequestHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<AddAttributeRequestHandler> logger)
    : IRequestHandler<AddAttributeRequest>
{
    public async Task Handle(AddAttributeRequest request, CancellationToken cancellationToken)
    {
        logger.LogInformation($"Trying to add attribute for category with id: {request.ParentId}");

        var parent = await unitOfWork.CategoryQueryRepository.GetByIdAsync(request.ParentId, cancellationToken);

        if (parent is null)
        {
            logger.LogError($"Category with id: {request.ParentId} not found");

            throw new BadRequestException("No such parent");
        }

        var attribute = mapper.Map<Category>(request);

        await unitOfWork.CategoryCommandRepository.AddAsync(attribute, cancellationToken);

        await unitOfWork.SaveAllAsync(cancellationToken);

        logger.LogInformation($"Attribute with id: {attribute.Id} successfully created for category: {attribute.ParentId}");
    }
}
