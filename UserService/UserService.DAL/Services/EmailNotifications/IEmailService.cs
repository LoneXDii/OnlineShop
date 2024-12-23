namespace UserService.DAL.Services.EmailNotifications;

public interface IEmailService
{
    Task SendEmailConfirmationCodeAsync(string email, string code);
    Task SendEmailConfirmationSucceededNotificationAsync(string email);
    Task SendPasswordResetCodeAsync(string email, string code);
    Task SendPasswordResetSucceededNotificationAsync(string email);
}