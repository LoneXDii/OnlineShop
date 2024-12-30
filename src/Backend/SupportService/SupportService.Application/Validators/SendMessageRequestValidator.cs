using FluentValidation;
using SupportService.Application.UseCases.SendMessage;

namespace SupportService.Application.Validators;

public class SendMessageRequestValidator : AbstractValidator<SendMessageRequest>
{
    public SendMessageRequestValidator()
    {
        RuleFor(req => req.Message.Text)
            .NotEmpty()
            .WithMessage("You should enter message text");

        RuleFor(req => req.Message.ChatId)
            .GreaterThan(0)
            .WithMessage("Wrong chat id");

        RuleFor(req => req.UserId)
            .NotEmpty()
            .WithMessage("Wrong user id");
    }
}
