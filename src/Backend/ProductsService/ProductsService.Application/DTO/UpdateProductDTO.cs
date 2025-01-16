using Microsoft.AspNetCore.Http;

namespace ProductsService.Application.DTO;

public class UpdateProductDTO
{
    public string? Name { get; set; }
    public double? Price { get; set; }
    public int? Quantity { get; set; }
    public IFormFile? Image { get; set; }
    public int[]? Attributes { get; set; }
}
