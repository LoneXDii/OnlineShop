using SendGrid;
using SendGrid.Helpers.Mail;
using System.Text;
using UserService.DAL.Models;

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

    public async Task SendUnconfirmedAccountDeletedNotificationAsync(string email)
    {
        var subject = "Your account wasn't created";
        var plainTextContent = $"Your account was deleted because you didn't confirm your email";
        var htmlContent = $"<span>{plainTextContent}</span>";
        var to = new EmailAddress(email);

        var msg = MailHelper.CreateSingleEmail(mailSender, to, subject, plainTextContent, htmlContent);

        await _sendGridClient.SendEmailAsync(msg);
    }

    public async Task SendEmailNotChangedNotificationAsync(string oldEmail, string newEmail)
    {
        var subject = "Your email wasn't changed";
        var plainTextContent = $"Your email wasn't changed because you didn't confirm your new email\nYour actual email is {oldEmail}";
        var htmlContent = $"<span>{plainTextContent}</span>";
        var toOld = new EmailAddress(oldEmail);
        var toNew = new EmailAddress(newEmail);

        var messageOld = MailHelper.CreateSingleEmail(mailSender, toOld, subject, plainTextContent, htmlContent);
        var messageNew = MailHelper.CreateSingleEmail(mailSender, toNew, subject, plainTextContent, htmlContent);

        await _sendGridClient.SendEmailAsync(messageOld);

        await _sendGridClient.SendEmailAsync(messageNew);
    }

    public async Task SendOrderStatusNotificationAsync(ConsumedOrder order)
    {
        var subject = "";

        switch (order.OrderStatus) 
        {
            case 0:
                subject = "Your order was created!";
                break;
            case 1:
                subject = "Your order was confirmed!";
                break;
            case 2:
                subject = "Your order was completed!";
                break;
            case 3:
                subject = "Your order was cancelled!";
                break;
        }

        var plainTextBuilder = new StringBuilder();
        var htmlBuilder = new StringBuilder();

        plainTextBuilder.AppendLine($"Заказ #{order.Id}.");
        plainTextBuilder.AppendLine($"Статус заказа: {order.OrderStatus}");
        plainTextBuilder.AppendLine($"Общая сумма: {order.TotalPrice}");
        plainTextBuilder.AppendLine("\nСписок товаров:");

        htmlBuilder.Append("<html><body>");
        htmlBuilder.Append("<h1>Информация о заказе</h1>");
        htmlBuilder.Append($"<p>Заказ #{order.Id}.</p>");
        htmlBuilder.Append($"<p>Статус заказа: {order.OrderStatus}</p>");
        htmlBuilder.Append($"<p>Общая сумма: {order.TotalPrice}</p>");
        htmlBuilder.Append("<h2>Список товаров:</h2>");
        htmlBuilder.Append("<table border='1' cellpadding='5' cellspacing='0'>");
        htmlBuilder.Append("<tr><th>Название</th><th>Количество</th><th>Цена</th><th>Скидка</th></tr>");

        foreach (var product in order.Products)
        {
            plainTextBuilder.AppendLine($"{product.Name} - Количество: {product.Quantity}, Цена: {product.Price}, Скидка: {product.Discount}%");

            htmlBuilder.Append("<tr>");
            htmlBuilder.Append($"<td>{product.Name}</td>");
            htmlBuilder.Append($"<td>{product.Quantity}</td>");
            htmlBuilder.Append($"<td>{product.Price}</td>");
            htmlBuilder.Append($"<td>{product.Discount}%</td>");
            htmlBuilder.Append("</tr>");
        }

        htmlBuilder.Append("</table>");
        htmlBuilder.Append("</body></html>");

        var plainTextContent = plainTextBuilder.ToString();
        var htmlContent = htmlBuilder.ToString();
        var to = new EmailAddress(order.UserEmail);

        var msg = MailHelper.CreateSingleEmail(mailSender, to, subject, plainTextContent, htmlContent);

        await _sendGridClient.SendEmailAsync(msg);
    }
}
