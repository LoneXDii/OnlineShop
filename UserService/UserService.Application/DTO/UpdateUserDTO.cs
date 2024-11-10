using Microsoft.AspNetCore.Http;

namespace UserService.Application.DTO;

public class UpdateUserDTO
{
	public string FirstName { get; set; }
	public string LastName { get; set; }
	public IFormFile? Avatar { get; set; }
}
