using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserService.BLL.DTO;
using UserService.BLL.Models;
using UserService.BLL.UseCases.UserUseCases.ResetPaswordUseCase;
using UserService.BLL.UseCases.UserUseCases.AskForResetPasswordUseCase;
using UserService.BLL.UseCases.UserUseCases.GetUserInfoUseCase;
using UserService.BLL.UseCases.UserUseCases.ListUsersWithPaginationUseCase;
using UserService.BLL.UseCases.UserUseCases.UpdateEmailUseCase;
using UserService.BLL.UseCases.UserUseCases.UpdatePasswordUseCase;
using UserService.BLL.UseCases.UserUseCases.UpdateUserUseCase;

namespace UserService.API.Controllers;

[Route("api/users")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;

    public UsersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> UpdateUser([FromForm] UpdateUserDTO userDTO)
    {
        var userId = HttpContext.User.FindFirst("Id").Value;

        await _mediator.Send(new UpdateUserRequest(userDTO, userId));

        return Ok();
    }

    [HttpPut("email")]
    [Authorize]
    public async Task<IActionResult> UpdateEmail([FromBody] EmailDTO newEmail)
    {
        var userId = HttpContext.User.FindFirst("Id").Value;

        await _mediator.Send(new UpdateEmailRequest(newEmail.Email, userId));

        return Ok();
    }

    [HttpPut("password")]
    [Authorize]
    public async Task<ActionResult<string>> UpdatePassword([FromBody] UpdatePasswordDTO updatePasswordDTO)
    {
        var userId = HttpContext.User.FindFirst("Id").Value;

        var token = await _mediator.Send(new UpdatePasswordRequest(updatePasswordDTO, userId));

        return Ok(token);
    }

    [HttpGet]
    [Authorize(Policy = "admin")]
    public async Task<ActionResult<PaginatedListModel<UserInfoDTO>>> ListUsers([FromQuery] PaginationDTO pagination)
    {
        var users = await _mediator.Send(new ListUsersWithPaginationRequest(pagination));

        return Ok(users);
    }

    [HttpGet("info")]
    [Authorize]
    public async Task<ActionResult<UserInfoDTO>> GetUserInfo()
    {
        var userId = HttpContext.User.FindFirst("Id").Value;

        var user = await _mediator.Send(new GetUserInfoRequest(userId));

        return user;
    }


    [HttpGet("{userId:regex(^[[a-fA-F0-9]]{{24}}$)}/info")]
    [Authorize(Policy = "admin")]
    public async Task<ActionResult<UserInfoDTO>> GetUserInfo([FromRoute] string userId)
    {
        var user = await _mediator.Send(new GetUserInfoRequest(userId));

        return user;
    }

    [HttpGet]
    [Route("password/resetting")]
    public async Task<IActionResult> AskForResetPassword([FromQuery] AskForResetPasswordRequest request)
    {
        await _mediator.Send(request);

        return Ok();
    }

    [HttpPost]
    [Route("password/resetting")]
    public async Task<ActionResult<string>> ResetPassword([FromBody] ResetPasswordRequest request)
    {
        await _mediator.Send(request);

        return Ok();
    }
}
