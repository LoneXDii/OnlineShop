using Microsoft.AspNetCore.Http;

namespace ProductsService.Application.DTO;

public class PostProductDTO
{
    public string Name { get; set; }
    public double Price { get; set; }
    public int Quantity { get; set; }
    public int CategoryId { get; set; }
    public List<AttributeValueDTO>? AttributeValues { get; set; }
    public IFormFile? Image { get; set; }
}
