﻿using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UserService.Application.DTO;
using UserService.Application.Models;
using UserService.Application.UseCases.UserUseCases.ListUsersWithPaginationUseCase;

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

	[HttpGet]
	[Route("pageNo={pageNo:int}&pageSize={pageSize:int}")]
	[Authorize(Policy = "admin")]
	public async Task<ActionResult<PaginatedListModel<UserInfoDTO>>> ListUsers(int pageNo, int pageSize)
	{
		var users = await _mediator.Send(new ListUsersWithPaginationRequest(pageNo, pageSize));
		return Ok(users);
	}
}
