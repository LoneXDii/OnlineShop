using MediatR;
using Microsoft.AspNetCore.Mvc;
using UserService.Application.UseCases.AuthUseCases.RefreshAccessTokenUseCase;

namespace UserService.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TokenController : ControllerBase
{
	private readonly IMediator _mediator;

	public TokenController(IMediator mediator)
	{
		_mediator = mediator;
	}

	[HttpGet]
	[Route("refresh/token={refreshToken}")]
	public async Task<ActionResult<string>> RefreshAccessToken(string refreshToken)
	{
		var token = await _mediator.Send(new RefreshAccessTokenRequest(refreshToken));
		return Ok(token);
	}
}
