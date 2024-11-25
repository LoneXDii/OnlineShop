namespace ProductsService.Application.DTO;

public class AddProductAttributeDTO
{
    public int ProductId { get; set; }
    public int AttributeId { get; set; }
    public string Value { get; set; }
}
