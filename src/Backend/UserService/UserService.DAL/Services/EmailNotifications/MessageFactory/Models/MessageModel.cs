namespace UserService.DAL.Services.EmailNotifications.MessageFactory.Models;

internal class MessageModel
{
    public string Subject { get; set; }
    public string PlaintTextContext { get; set; }
    public string HtmlContent { get; set; }
}
