using FluentValidation;
using UserService.BLL.DTO;

namespace UserService.BLL.Validators;

public class UpdateEmailDTOValidator : AbstractValidator<UpdateEmailDTO>
{
    public UpdateEmailDTOValidator()
    {
        RuleFor(email => email.Email)
            .NotEmpty()
            .Matches(@"^\S+@\S+\.\S+$")
            .WithMessage("Incorrect email adress");
    }
}
