using OrderService.Domain.Common.Statuses;
using OrderService.Domain.Entities;

namespace OrderService.Application.DTO;

public class PostOrderDTO
{
	public int OrderStatus { get; set; }
	public int PaymentStatus { get; set; }
	public string UserId { get; set; }
	public double TotalPrice { get; set; }
	public DateTime CreatedDate { get; set; }
	public List<Product> Products { get; set; }
}
