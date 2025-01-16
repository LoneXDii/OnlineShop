using FluentValidation;
using SupportService.Application.UseCases.CreateChat;

namespace SupportService.Application.Validators;

public class CreateChatRequestValidator : AbstractValidator<CreateChatRequest>
{
    public CreateChatRequestValidator()
    {
        RuleFor(req => req.ClientId)
            .NotEmpty()
            .WithMessage("Wrong client id");

        RuleFor(req => req.ClientName)
            .NotEmpty()
            .WithMessage("Wrong client name");
    }
}
