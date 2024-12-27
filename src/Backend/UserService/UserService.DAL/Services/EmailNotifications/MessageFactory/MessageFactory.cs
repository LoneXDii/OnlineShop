using System.Text;
using UserService.DAL.Models;
using UserService.DAL.Services.EmailNotifications.MessageFactory.Models;

namespace UserService.DAL.Services.EmailNotifications.MessageFactory;

internal class MessageFactory : IMessageFactory
{
    public MessageModel CreateEmailConfirmationMessage(string code)
    {
        var subject = "Email verification";
        var plainTextContent = $"Your email confirmation code is: {code}";
        var htmlContent = $"<span>{plainTextContent}</span>";

        return new MessageModel
        {
            Subject = subject,
            PlaintTextContext = plainTextContent,
            HtmlContent = htmlContent
        };
    }

    public MessageModel CreateEmailConfirmationSucceedMessage()
    {
        var subject = "Email succesfully verified!";
        var plainTextContent = $"Thank you for registration in our shop!";
        var htmlContent = $"<span>{plainTextContent}</span>";

        return new MessageModel
        {
            Subject = subject,
            PlaintTextContext = plainTextContent,
            HtmlContent = htmlContent
        };
    }

    public MessageModel CreatePasswordResetMessage(string code)
    {
        var subject = "Password reset";
        var plainTextContent = $"Your password reset code is: {code}";
        var htmlContent = $"<span>{plainTextContent}</span>";

        return new MessageModel
        {
            Subject = subject,
            PlaintTextContext = plainTextContent,
            HtmlContent = htmlContent
        };  
    }

    public MessageModel CreatePasswordResetSucceedMessage()
    {
        var subject = "Password successfully changed!";
        var plainTextContent = $"Your password has been changed!";
        var htmlContent = $"<span>{plainTextContent}</span>";

        return new MessageModel
        {
            Subject = subject,
            PlaintTextContext = plainTextContent,
            HtmlContent = htmlContent
        };
    }

    public MessageModel CreateEmailConfirmationFailedMessage()
    {
        var subject = "Your account wasn't created";
        var plainTextContent = $"Your account was deleted because you didn't confirm your email";
        var htmlContent = $"<span>{plainTextContent}</span>";

        return new MessageModel
        {
            Subject = subject,
            PlaintTextContext = plainTextContent,
            HtmlContent = htmlContent
        };
    }

    public MessageModel CreateEmailChangeFailedMessage(string oldEmail)
    {
        var subject = "Your email wasn't changed";
        var plainTextContent = $"Your email wasn't changed because you didn't confirm your new email\nYour actual email is {oldEmail}";
        var htmlContent = $"<span>{plainTextContent}</span>";

        return new MessageModel
        {
            Subject = subject,
            PlaintTextContext = plainTextContent,
            HtmlContent = htmlContent
        };
    }

    public MessageModel CreateOrderStatusChangedMessage(ConsumedOrder order)
    {
        return new MessageModel
        {
            Subject = CreateOrderStatusChangedMessageSubject(order.OrderStatus),
            PlaintTextContext = CreateOrderStatusChangedMessagePlainTextContent(order),
            HtmlContent = CreateOrderStatusChangedMessageHtmlContent(order)
        };
    }

    private string CreateOrderStatusChangedMessageSubject(int status)
    {
        switch (status)
        {
            case 0:
                return "Your order was created!";
            case 1:
                return "Your order was confirmed!";
            case 2:
                return "Your order was completed!";
            case 3:
                return "Your order was cancelled!";
            default:
                return "Unknown order status";
        }
    }

    private string CreateOrderStatusChangedMessagePlainTextContent(ConsumedOrder order)
    {
        var plainTextBuilder = new StringBuilder();

        plainTextBuilder.AppendLine($"Order #{order.Id}.");
        plainTextBuilder.AppendLine($"Order Status: {order.OrderStatus}");
        plainTextBuilder.AppendLine($"Total Amount: {order.TotalPrice}");
        plainTextBuilder.AppendLine("\nProduct List:");

        foreach (var product in order.Products)
        {
            plainTextBuilder.Append($"{product.Name} ");
            plainTextBuilder.Append($"- Quantity: {product.Quantity}, ");
            plainTextBuilder.Append($"Price: {product.Price}, ");
            plainTextBuilder.Append($"Discount: {product.Discount}%, ");
            plainTextBuilder.Append($"Discounted Price: {product.Price * (1 - (double)product.Discount / 100)}, ");
            plainTextBuilder.AppendLine($"Total: {product.Price * product.Quantity * (1 - product.Discount / 100)}");
        }

        return plainTextBuilder.ToString();
    }

    private string CreateOrderStatusChangedMessageHtmlContent(ConsumedOrder order)
    {
        var htmlBuilder = new StringBuilder();

        htmlBuilder.Append("<html><body>");
        htmlBuilder.Append("<h1>Order Information</h1>");
        htmlBuilder.Append($"<p>Order #{order.Id}.</p>");
        htmlBuilder.Append($"<p>Order Status: {order.OrderStatus}</p>");
        htmlBuilder.Append($"<p>Total Amount: {order.TotalPrice}</p>");
        htmlBuilder.Append("<h2>Product List:</h2>");
        htmlBuilder.Append("<table border='1' cellpadding='5' cellspacing='0'>");
        htmlBuilder.Append("<tr><th>Name</th><th>Quantity</th><th>Price</th><th>Discount</th><th>Discounted Price</th><th>Total</th></tr>");

        foreach (var product in order.Products)
        {
            htmlBuilder.Append("<tr>");
            htmlBuilder.Append($"<td>{product.Name}</td>");
            htmlBuilder.Append($"<td>{product.Quantity}</td>");
            htmlBuilder.Append($"<td>{product.Price}</td>");
            htmlBuilder.Append($"<td>{product.Discount}%</td>");
            htmlBuilder.Append($"<td>{product.Price * (1 - (double)product.Discount / 100)}</td>");
            htmlBuilder.Append($"<td>{product.Price * product.Quantity * (1 - (double)product.Discount / 100)}</td>");
            htmlBuilder.Append("</tr>");
        }

        htmlBuilder.Append("</table>");
        htmlBuilder.Append("</body></html>");

        return htmlBuilder.ToString();
    }
}
