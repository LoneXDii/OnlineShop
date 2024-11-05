using Microsoft.AspNetCore.Identity;

namespace UserService.Domain.Entities;

public class AppUser : IdentityUser
{
	public string FirstName { get; set; }
	public string LastName { get; set; }
	public string? AvatarUrl { get; set; }
}
