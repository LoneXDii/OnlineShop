using Microsoft.AspNetCore.Http;

namespace UserService.Application.Models;

public class RegisterModel
{
	public string FirstName { get; set; }
	public string LastName { get; set; }
	public string Email { get; set; }
	public string Password { get; set; }
	public IFormFile? Avatar { get; set; }
}
