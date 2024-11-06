using SendGrid;
using SendGrid.Helpers.Mail;

namespace UserService.Infrastructure.Services.EmailNotifications;

internal class EmailService : IEmailService
{
	private readonly ISendGridClient _sendGridClient;

	public EmailService(ISendGridClient sendGridClient)
	{
		_sendGridClient = sendGridClient;
	}
}
