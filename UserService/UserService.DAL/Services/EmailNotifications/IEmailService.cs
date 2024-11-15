using UserService.DAL.Entities;

namespace UserService.DAL.Services.EmailNotifications;

public interface IEmailService
{
    Task SendEmailConfirmationCodeAsync(string email, string confirmationLink);
    Task SendEmailConfirmationSucceededNotificationAsync(string email);
}