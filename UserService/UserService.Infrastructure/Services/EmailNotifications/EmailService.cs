using SendGrid;
using SendGrid.Helpers.Mail;

namespace UserService.Infrastructure.Services.EmailNotifications;

internal class EmailService : IEmailService
{
	private readonly ISendGridClient _sendGridClient;
	private readonly EmailAddress mailSender;

	public EmailService(ISendGridClient sendGridClient)
	{
		_sendGridClient = sendGridClient;
		mailSender = new EmailAddress("myshopemailsender@gmail.com", "Notification");
	}

	public async Task SendEmailConfirmationCodeAsync(string email, string confirmationLink)
	{
		var subject = "Email verification";
		var plainTextContent = $"Please enter this link to confirm your email: {confirmationLink}";
		var htmlContent = $"<span>{plainTextContent}</span>";
		var to = new EmailAddress(email);

		var msg = MailHelper.CreateSingleEmail(mailSender, to, subject, plainTextContent, htmlContent);

		await _sendGridClient.SendEmailAsync(msg);
	}

	public async Task SendEmailConfirmationSucceededNotificationAsync(string email)
	{
		var subject = "Email succesfully verificated!";
		var plainTextContent = $"Thank you for registration in our shop!";
		var htmlContent = $"<span>{plainTextContent}</span>";
		var to = new EmailAddress(email);

		var msg = MailHelper.CreateSingleEmail(mailSender, to, subject, plainTextContent, htmlContent);

		await _sendGridClient.SendEmailAsync(msg);
	}
}
