using MediatR;
using Microsoft.AspNetCore.Mvc;
using UserService.Application.Models;
using UserService.Application.UseCases.AuthUseCases.LoginUserUseCase;
using UserService.Application.UseCases.AuthUseCases.RegisterUserUseCase;
using UserService.Infrastructure.Models;

namespace UserService.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AccountController : ControllerBase
{
	private readonly IMediator _mediator;

	public AccountController(IMediator mediator)
	{
		_mediator = mediator;
	}

	[HttpPost]
	[Route("register")]
	public async Task<IActionResult> Register([FromForm] RegisterModel registerModel)
	{
		await _mediator.Send(new RegisterUserRequest(registerModel));
		return Ok();
	}

	[HttpPost]
	[Route("login")]
	public async Task<ActionResult<TokensDTO>> Login(LoginModel loginModel)
	{
		var tokens = await _mediator.Send(new LoginUserRequest(loginModel));
		return Ok(tokens);
	}

}
