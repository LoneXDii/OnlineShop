using FluentValidation;
using SupportService.Application.UseCases.CloseChat;

namespace SupportService.Application.Validators;

public class CloseChatRequestValidator : AbstractValidator<CloseChatRequest>
{
    public CloseChatRequestValidator()
    {
        RuleFor(req => req.UserId)
            .NotEmpty()
            .WithMessage("Wrong user Id")
            .When(req => req.UserId is not null);

        RuleFor(req => req.ChatId)
            .GreaterThan(0)
            .WithMessage("Wrong chat Id");
    }
}
