namespace ProductsService.Application.DTO;

public class ResponseProductDTO
{
    public int Id { get; set; }
    public string Name { get; set; }
    public double Price { get; set; }
    public int Quantity { get; set; }
    public string? ImageUrl { get; set; }
    public DiscountDTO? Discount { get; set; }
    public ResponseCategoryDTO Category { get; set; }
    public List<ResponseAttributeValueDTO> AttributeValues { get; set; }
}
