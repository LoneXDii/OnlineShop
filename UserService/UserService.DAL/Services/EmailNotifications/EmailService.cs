using SendGrid;
using SendGrid.Helpers.Mail;

namespace UserService.DAL.Services.EmailNotifications;

internal class EmailService : IEmailService
{
    private readonly ISendGridClient _sendGridClient;
    private readonly EmailAddress mailSender;

    public EmailService(ISendGridClient sendGridClient)
    {
        _sendGridClient = sendGridClient;
        mailSender = new EmailAddress("myshopemailsender@gmail.com", "Notification");
    }

    public async Task SendEmailConfirmationCodeAsync(string email, string code)
    {
        var subject = "Email verification";
        var plainTextContent = $"Your email confirmation code is: {code}";
        var htmlContent = $"<span>{plainTextContent}</span>";
        var to = new EmailAddress(email);

        var msg = MailHelper.CreateSingleEmail(mailSender, to, subject, plainTextContent, htmlContent);

        await _sendGridClient.SendEmailAsync(msg);
    }

    public async Task SendEmailConfirmationSucceededNotificationAsync(string email)
    {
        var subject = "Email succesfully verified!";
        var plainTextContent = $"Thank you for registration in our shop!";
        var htmlContent = $"<span>{plainTextContent}</span>";
        var to = new EmailAddress(email);

        var msg = MailHelper.CreateSingleEmail(mailSender, to, subject, plainTextContent, htmlContent);

        await _sendGridClient.SendEmailAsync(msg);
    }

    public async Task SendPasswordResetCodeAsync(string email, string code)
    {
        var subject = "Password reset";
        var plainTextContent = $"Your password reset code is: {code}";
        var htmlContent = $"<span>{plainTextContent}</span>";
        var to = new EmailAddress(email);

        var msg = MailHelper.CreateSingleEmail(mailSender, to, subject, plainTextContent, htmlContent);

        await _sendGridClient.SendEmailAsync(msg);
    }

    public async Task SendPasswordResetSucceededNotificationAsync(string email)
    {
        var subject = "Password successfully changed!";
        var plainTextContent = $"Your password has been changed!";
        var htmlContent = $"<span>{plainTextContent}</span>";
        var to = new EmailAddress(email);

        var msg = MailHelper.CreateSingleEmail(mailSender, to, subject, plainTextContent, htmlContent);

        await _sendGridClient.SendEmailAsync(msg);
    }
}
