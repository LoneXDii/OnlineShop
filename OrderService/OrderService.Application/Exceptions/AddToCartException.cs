namespace OrderService.Application.Exceptions;

public class AddToCartException : Exception
{
	public AddToCartException(string message) : base(message) { }
}