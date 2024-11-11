namespace OrderService.Domain.Models;

public class Order
{
	public string? Id { get; set; }
	public string OrderStatus { get; set; }
	public string PaymentStatus { get; set; }
	public string UserId { get; set; }
	public double TotalPrice { get; set; }
	public List<Product> Products { get; set; }
}
