using OrderService.Domain.Entities;

namespace OrderService.Application.DTO;

public class CartDTO
{
	public int Count { get; set; }
	public int TotalCost { get; set; }
	public List<Product> Products {  get; set; } 
}
