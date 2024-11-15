using Microsoft.AspNetCore.Http;

namespace UserService.BLL.DTO;

public class UpdateUserDTO
{
	public string FirstName { get; set; }
	public string LastName { get; set; }
	public IFormFile? Avatar { get; set; }
}
