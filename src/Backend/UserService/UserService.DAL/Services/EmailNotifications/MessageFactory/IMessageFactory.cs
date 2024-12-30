using UserService.DAL.Models;
using UserService.DAL.Services.EmailNotifications.MessageFactory.Models;

namespace UserService.DAL.Services.EmailNotifications.MessageFactory;

internal interface IMessageFactory
{
    public MessageModel CreateEmailConfirmationMessage(string code);
    public MessageModel CreateEmailConfirmationSucceedMessage();
    public MessageModel CreatePasswordResetMessage(string code);
    public MessageModel CreatePasswordResetSucceedMessage();
    public MessageModel CreateEmailConfirmationFailedMessage();
    public MessageModel CreateEmailChangeFailedMessage(string oldEmail);
    public MessageModel CreateOrderStatusChangedMessage(ConsumedOrder order);
}
