namespace OrderService.Application.Exceptions;

public class NotInCartException : Exception
{
    public NotInCartException(string message) : base(message) { }
}
