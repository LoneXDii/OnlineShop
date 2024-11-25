using Microsoft.AspNetCore.Http;

namespace ProductsService.Application.DTO;

public class UpdateProductDTO
{
	public int Id { get; set; }
	public string Name { get; set; }
	public double Price { get; set; }
	public int Quantity { get; set; }
	public int CategoryId { get; set; }
	public IFormFile? Image { get; set; }
}
