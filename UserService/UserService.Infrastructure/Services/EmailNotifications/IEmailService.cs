using UserService.Domain.Entities;

namespace UserService.Infrastructure.Services.EmailNotifications;

public interface IEmailService
{
	Task SendEmailConfirmationCodeAsync(string email, string confirmationLink);
	Task SendEmailConfirmationSucceededNotificationAsync(string email);
}