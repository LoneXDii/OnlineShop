using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UserService.Application.DTO;
using UserService.Application.Models;
using UserService.Application.UseCases.UserUseCases.GetUserInfoUseCase;
using UserService.Application.UseCases.UserUseCases.ListUsersWithPaginationUseCase;
using UserService.Application.UseCases.UserUseCases.UpdateEmailUseCase;
using UserService.Application.UseCases.UserUseCases.UpdatePasswordUseCase;
using UserService.Application.UseCases.UserUseCases.UpdateUserUseCase;

namespace UserService.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
	private readonly IMediator _mediator;

	public UsersController(IMediator mediator)
	{
		_mediator = mediator;
	}

	[HttpPost]
	[Route("update")]
	[Authorize]
	public async Task<IActionResult> UpdateUser([FromForm] UpdateUserDTO userDTO)
	{
		var userId = HttpContext.User.FindFirst("Id").Value;
		await _mediator.Send(new UpdateUserRequest(userDTO, userId));
		return Ok();
	}

	[HttpPost]
	[Route("update/email")]
	[Authorize]
	public async Task<IActionResult> UpdateEmail(string newEmail)
	{
		var userId = HttpContext.User.FindFirst("Id").Value;
		await _mediator.Send(new UpdateEmailRequest(newEmail, userId));
		return Ok();
	}

	[HttpPost]
	[Route("update/password")]
	[Authorize]
	public async Task<ActionResult<string>> UpdatePassword(UpdatePasswordDTO updatePasswordDTO)
	{
		var userId = HttpContext.User.FindFirst("Id").Value;
		var token = await _mediator.Send(new UpdatePasswordRequest(updatePasswordDTO, userId));
		return Ok(token);
	}

	[HttpGet]
	[Route("pageNo={pageNo:int}&pageSize={pageSize:int}")]
	[Authorize(Policy = "admin")]
	public async Task<ActionResult<PaginatedListModel<UserInfoDTO>>> ListUsers(int pageNo, int pageSize)
	{
		var users = await _mediator.Send(new ListUsersWithPaginationRequest(pageNo, pageSize));
		return Ok(users);
	}

	[HttpGet]
	[Route("info")]
	[Authorize]
	public async Task<ActionResult<UserInfoDTO>> GetUserInfo()
	{
		var userId = HttpContext.User.FindFirst("Id").Value;
		var user = await _mediator.Send(new GetUserInfoRequest(userId));
		return user;
	}

	[HttpGet]
	[Route("info/id={userId}")]
	[Authorize(Policy = "admin")]
	public async Task<ActionResult<UserInfoDTO>> GetUserInfo(string userId)
	{
		var user = await _mediator.Send(new GetUserInfoRequest(userId));
		return user;
	}
}
