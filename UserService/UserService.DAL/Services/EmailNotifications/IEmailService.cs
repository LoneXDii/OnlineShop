namespace UserService.DAL.Services.EmailNotifications;

public interface IEmailService
{
    Task SendEmailConfirmationCodeAsync(string email, string code);
    Task SendEmailConfirmationSucceededNotificationAsync(string email);
}