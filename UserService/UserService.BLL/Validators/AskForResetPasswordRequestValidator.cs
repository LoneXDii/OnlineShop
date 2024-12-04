using FluentValidation;
using UserService.BLL.UseCases.UserUseCases.AskForResetPasswordUseCase;

namespace UserService.BLL.Validators;

public class AskForResetPasswordRequestValidator : AbstractValidator<AskForResetPasswordRequest>
{
    public AskForResetPasswordRequestValidator()
    {
        RuleFor(req => req.Email)
            .NotEmpty()
            .Matches(@"^\S+@\S+\.\S+$")
            .WithMessage("Incorrect email adress");
    }
}
