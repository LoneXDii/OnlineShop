using Microsoft.AspNetCore.Http;

namespace UserService.BLL.DTO;

public class RegisterDTO
{
	public string FirstName { get; set; }
	public string LastName { get; set; }
	public string Email { get; set; }
	public string Password { get; set; }
	public IFormFile? Avatar { get; set; }
}
