using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserService.Infrastructure.Services.EmailNotifications;

namespace UserService.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DebugController : ControllerBase
{
	[HttpGet]
	[Route("auth")]
	[Authorize]
	public IActionResult Auth()
	{
		return Ok("Authorized");
	}
}
