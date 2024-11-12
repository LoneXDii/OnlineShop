namespace OrderService.Application.Exceptions;

public class OrderException : Exception
{
	public OrderException(string message) : base(message) { }
}
