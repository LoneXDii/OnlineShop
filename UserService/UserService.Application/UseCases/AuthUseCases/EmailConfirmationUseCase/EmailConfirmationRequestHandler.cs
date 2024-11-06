using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Web;
using UserService.Domain.Entities;
using UserService.Infrastructure.Services.EmailNotifications;

namespace UserService.Application.UseCases.AuthUseCases.EmailConfirmationUseCase;

internal class EmailConfirmationRequestHandler(UserManager<AppUser> userManager, IEmailService emailService)
	: IRequestHandler<EmailConfirmationRequest>
{
	public async Task Handle(EmailConfirmationRequest request, CancellationToken cancellationToken)
	{
		var user = await userManager.FindByEmailAsync(request.email);
		if (user is null)
		{
			throw new NotImplementedException();
		}

		var code = HttpUtility.UrlDecode(request.code);
		code = code.Replace(" ", "+");
		var result = await userManager.ConfirmEmailAsync(user, code);

		if (!result.Succeeded)
		{
			throw new NotImplementedException();
		}

		await emailService.SendEmailConfirmationSucceededNotificationAsync(user.Email);
	}
}
