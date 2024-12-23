namespace ProductsService.Application.DTO;

public class CategoryAttributesValuesDTO
{
    public ResponseCategoryDTO Attribute { get; set; }
    public List<ResponseCategoryDTO> Values { get; set; }
}
