namespace ProductsService.Application.DTO;

public class ProductDTO
{
    public int Id { get; set; }
    public string Name { get; set; }
    public double Price { get; set; }
    public int Quantity { get; set; }
    public string? ImageUrl { get; set; }
    public int? CategoryId { get; set; }
    public CategoryDTO Category { get; set; }
    public List<AttributeValueDTO> AttributeValues { get; set; }
}
