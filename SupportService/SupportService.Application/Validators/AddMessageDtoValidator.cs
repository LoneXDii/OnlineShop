using FluentValidation;
using SupportService.Application.DTO;

namespace SupportService.Application.Validators;

public class AddMessageDtoValidator : AbstractValidator<AddMessageDTO>
{
    public AddMessageDtoValidator()
    {
        RuleFor(message => message.Text)
            .NotEmpty()
            .WithMessage("You should enter message text");

        RuleFor(message => message.ChatId)
            .GreaterThan(0)
            .WithMessage("Wrong chat id");
    }
}
