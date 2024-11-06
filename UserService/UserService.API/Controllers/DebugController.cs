using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
