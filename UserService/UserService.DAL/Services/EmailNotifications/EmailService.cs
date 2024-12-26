using SendGrid;
using SendGrid.Helpers.Mail;
using UserService.DAL.Models;
using UserService.DAL.Services.EmailNotifications.MessageFactory;

namespace UserService.DAL.Services.EmailNotifications;

internal class EmailService : IEmailService
{
    private readonly ISendGridClient _sendGridClient;
    private readonly IMessageFactory _messageFactory;
    private readonly EmailAddress mailSender;

    public EmailService(ISendGridClient sendGridClient, IMessageFactory messageFactory)
    {
        _sendGridClient = sendGridClient;
        _messageFactory = messageFactory;
        mailSender = new EmailAddress("myshopemailsender@gmail.com", "Notification");
    }

    public async Task SendEmailConfirmationCodeAsync(string email, string code)
    {
        var messageModel = _messageFactory.CreateEmailConfirmationMessage(code);
        var to = new EmailAddress(email);

        var msg = MailHelper.CreateSingleEmail(mailSender, to, messageModel.Subject, messageModel.PlaintTextContext, messageModel.HtmlContent);

        await _sendGridClient.SendEmailAsync(msg);
    }

    public async Task SendEmailConfirmationSucceededNotificationAsync(string email)
    {
        var messageModel = _messageFactory.CreateEmailConfirmationSucceedMessage();
        var to = new EmailAddress(email);

        var msg = MailHelper.CreateSingleEmail(mailSender, to, messageModel.Subject, messageModel.PlaintTextContext, messageModel.HtmlContent);

        await _sendGridClient.SendEmailAsync(msg);
    }

    public async Task SendPasswordResetCodeAsync(string email, string code)
    {
        var messageModel = _messageFactory.CreatePasswordResetMessage(code);
        var to = new EmailAddress(email);

        var msg = MailHelper.CreateSingleEmail(mailSender, to, messageModel.Subject, messageModel.PlaintTextContext, messageModel.HtmlContent);

        await _sendGridClient.SendEmailAsync(msg);
    }

    public async Task SendPasswordResetSucceededNotificationAsync(string email)
    {
        var messageModel = _messageFactory.CreatePasswordResetSucceedMessage();
        var to = new EmailAddress(email);

        var msg = MailHelper.CreateSingleEmail(mailSender, to, messageModel.Subject, messageModel.PlaintTextContext, messageModel.HtmlContent);

        await _sendGridClient.SendEmailAsync(msg);
    }

    public async Task SendUnconfirmedAccountDeletedNotificationAsync(string email)
    {
        var messageModel = _messageFactory.CreateEmailConfirmationFailedMessage();
        var to = new EmailAddress(email);

        var msg = MailHelper.CreateSingleEmail(mailSender, to, messageModel.Subject, messageModel.PlaintTextContext, messageModel.HtmlContent);

        await _sendGridClient.SendEmailAsync(msg);
    }

    public async Task SendEmailNotChangedNotificationAsync(string oldEmail, string newEmail)
    {
        var messageModel = _messageFactory.CreateEmailChangeFailedMessage(oldEmail);
        var toOld = new EmailAddress(oldEmail);
        var toNew = new EmailAddress(newEmail);

        var messageOld = MailHelper.CreateSingleEmail(mailSender, toOld, messageModel.Subject, messageModel.PlaintTextContext, messageModel.HtmlContent);
        var messageNew = MailHelper.CreateSingleEmail(mailSender, toNew, messageModel.Subject, messageModel.PlaintTextContext, messageModel.HtmlContent);

        await _sendGridClient.SendEmailAsync(messageOld);

        await _sendGridClient.SendEmailAsync(messageNew);
    }

    public async Task SendOrderStatusNotificationAsync(ConsumedOrder order)
    {
        var messageModel = _messageFactory.CreateOrderStatusChangedMessage(order);
        var to = new EmailAddress(order.UserEmail);

        var msg = MailHelper.CreateSingleEmail(mailSender, to, messageModel.Subject, messageModel.PlaintTextContext, messageModel.HtmlContent);

        await _sendGridClient.SendEmailAsync(msg);
    }
}
