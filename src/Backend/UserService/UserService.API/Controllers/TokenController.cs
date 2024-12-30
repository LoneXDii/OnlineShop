using MediatR;
using Microsoft.AspNetCore.Mvc;
using UserService.BLL.UseCases.AuthUseCases.RefreshAccessTokenUseCase;

namespace UserService.API.Controllers;

[Route("api/tokens")]
[ApiController]
public class TokenController : ControllerBase
{
    private readonly IMediator _mediator;

    public TokenController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [Route("refresh")]
    public async Task<ActionResult<string>> RefreshAccessToken([FromQuery] string refreshToken)
    {
        var token = await _mediator.Send(new RefreshAccessTokenRequest(refreshToken));

        return Ok(token);
    }
}
