using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserService.BLL.DTO;
using UserService.BLL.UseCases.AuthUseCases.EmailConfirmationUseCase;
using UserService.BLL.UseCases.AuthUseCases.LoginUserUseCase;
using UserService.BLL.UseCases.AuthUseCases.LogoutUserUseCase;
using UserService.BLL.UseCases.AuthUseCases.RegisterUserUseCase;
using UserService.DAL.Models;

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
	public async Task<IActionResult> Register([FromForm] RegisterDTO registerModel)
	{
		await _mediator.Send(new RegisterUserRequest(registerModel));

		return Ok();
	}

	[HttpPost]
	[Route("login")]
	public async Task<ActionResult<TokensDTO>> Login(LoginDTO loginModel)
	{
		var tokens = await _mediator.Send(new LoginUserRequest(loginModel));

		return Ok(tokens);
	}

	[HttpGet]
	[Route("confirm/email={email}&code={code}")]
	public async Task<IActionResult> ConfirmEmail(string email, string code)
	{
		await _mediator.Send(new EmailConfirmationRequest(email, code));

		return Ok("Email confirmed");
	}

	[HttpGet]
	[Route("logout")]
	[Authorize]
	public async Task<IActionResult> Logout()
	{
		var userId = HttpContext.User.FindFirst("Id").Value;

		await _mediator.Send(new LogoutUserRequest(userId));

		return Ok();
	}
}
