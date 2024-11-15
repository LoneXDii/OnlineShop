using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Web;
using UserService.DAL.Entities;
using UserService.BLL.Exceptions;
using UserService.DAL.Services.EmailNotifications;

namespace UserService.BLL.UseCases.AuthUseCases.EmailConfirmationUseCase;

internal class EmailConfirmationRequestHandler(UserManager<AppUser> userManager, IEmailService emailService)
	: IRequestHandler<EmailConfirmationRequest>
{
	public async Task Handle(EmailConfirmationRequest request, CancellationToken cancellationToken)
	{
		var user = await userManager.FindByEmailAsync(request.email);

		if (user is null)
		{
			throw new NotFoundException("No user with such email");
		}

		var code = HttpUtility.UrlDecode(request.code);
		code = code.Replace(" ", "+");

		var result = await userManager.ConfirmEmailAsync(user, code);

		if (!result.Succeeded)
		{
			throw new BadRequestException($"{result.Errors}");
		}

		await emailService.SendEmailConfirmationSucceededNotificationAsync(user.Email);
	}
}
