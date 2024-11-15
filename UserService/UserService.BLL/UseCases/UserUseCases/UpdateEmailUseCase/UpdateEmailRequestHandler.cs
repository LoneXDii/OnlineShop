using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Text.RegularExpressions;
using System.Web;
using UserService.DAL.Entities;
using UserService.BLL.Exceptions;
using UserService.DAL.Services.EmailNotifications;

namespace UserService.BLL.UseCases.UserUseCases.UpdateEmailUseCase;

internal class UpdateEmailRequestHandler(UserManager<AppUser> userManager, IEmailService emailService)
	: IRequestHandler<UpdateEmailRequest>
{
	public async Task Handle(UpdateEmailRequest request, CancellationToken cancellationToken)
	{
		if (!Regex.IsMatch(request.newEmail, @"^\S+@\S+\.\S+$"))
		{
			throw new BadRequestException("Incorrect email");
		}

		var userWithNewEmail = await userManager.FindByEmailAsync(request.newEmail);

		if (userWithNewEmail is not null)
		{
			throw new BadRequestException("This email is already in use");
		}

		var user = await userManager.FindByIdAsync(request.userId);

		if (user is null)
		{
			throw new NotFoundException("No such user");
		}

		user.Email = request.newEmail;
		user.UserName = request.newEmail;
		user.EmailConfirmed = false;
		await userManager.UpdateAsync(user);

		var code = await userManager.GenerateEmailConfirmationTokenAsync(user);

		code = HttpUtility.UrlEncode(code);
		var email = HttpUtility.UrlEncode(user.Email);
		var confirmationLink = $"https://localhost:7001/api/account/confirm/email={email}&code={code}";

		await emailService.SendEmailConfirmationCodeAsync(user.Email, confirmationLink);
	}
}
