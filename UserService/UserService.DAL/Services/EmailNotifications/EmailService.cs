using SendGrid;
using SendGrid.Helpers.Mail;
using System.Web;

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
        code = HttpUtility.UrlEncode(code);
        var encodedEmail = HttpUtility.UrlEncode(email);
        var confirmationLink = $"https://localhost:7001/api/account/confirm/email={encodedEmail}&code={code}";

        var subject = "Email verification";
        var plainTextContent = $"Please enter this link to confirm your email: {confirmationLink}";
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
}
