using AutoMapper;
using MediatR;
using ProductsService.Application.Exceptions;
using ProductsService.Domain.Abstractions.Database;
using ProductsService.Domain.Entities;

namespace ProductsService.Application.UseCases.CategoryUseCases.Commands.AddAttribute;

internal class AddAttributeRequestHandler(IUnitOfWork unitOfWork, IMapper mapper)
    : IRequestHandler<AddAttributeRequest>
{
    public async Task Handle(AddAttributeRequest request, CancellationToken cancellationToken)
    {
        var parent = await unitOfWork.CategoryQueryRepository.GetByIdAsync(request.ParentId, cancellationToken);

        if (parent is null)
        {
            throw new BadRequestException("No such parent");
        }

        var attribute = mapper.Map<Category>(request);
        //DELETE THIS
        attribute.NormalizedName = "";

        await unitOfWork.CategoryCommandRepository.AddAsync(attribute, cancellationToken);

        await unitOfWork.SaveAllAsync(cancellationToken);
    }
}
