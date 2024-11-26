namespace ProductsService.Application.DTO;

public class CategoryAttributesValuesDTO
{
    public CategoryDTO Attribute { get; set; }
    public List<CategoryDTO> Values { get; set; }
}
