using Microsoft.AspNetCore.Http;

namespace ProductsService.Application.DTO;

public class UpdateCategoryDTO
{
    public string Name { get; set; }
    public IFormFile? Image { get; set; }
}
