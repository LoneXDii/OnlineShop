using FluentValidation;
using ProductsService.Application.DTO;

namespace ProductsService.Application.Validators.Categories;

public class UpdateAttributeDtoValidator : AbstractValidator<UpdateAttributeDTO>
{
    public UpdateAttributeDtoValidator()
    {
        RuleFor(req => req.AttributeId)
            .GreaterThan(0)
            .WithMessage("Wrong attribute id");
    }
}
