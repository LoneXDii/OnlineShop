using OrderService.Domain.Entities;

namespace OrderService.Application.DTO;

public class CartDTO
{
	public int Count { get; set; }
	public double TotalCost { get; set; }
	public List<Product> Products { get; set; } 
}
