namespace ProductsService.Application.DTO;

public class CategoryDTO
{
	public int Id { get; set; }
	public string Name { get; set; }
	public string NormalizedName { get; set; }
	public string? ImageUrl { get; set; }
}
