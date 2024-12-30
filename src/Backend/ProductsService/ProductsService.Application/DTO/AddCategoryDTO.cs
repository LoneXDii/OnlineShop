using Microsoft.AspNetCore.Http;

namespace ProductsService.Application.DTO;

public class AddCategoryDTO
{
    public string Name { get; set; }
    public IFormFile? Image { get; set; }
}
