using FluentValidation;
using UserService.BLL.UseCases.AuthUseCases.ResendEmailConfirmationCodeUseCase;

namespace UserService.BLL.Validators;

public class ResendEmailConfirmationCodeRequestValidator : AbstractValidator<ResendEmailConfirmationCodeRequest>
{
    public ResendEmailConfirmationCodeRequestValidator()
    {
        RuleFor(req => req.Email)
            .NotEmpty()
            .Matches(@"^\S+@\S+\.\S+$")
            .WithMessage("Incorrect email adress");
    }
}
