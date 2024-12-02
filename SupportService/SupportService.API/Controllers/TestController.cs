using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SupportService.Application.UseCases.CloseChat;
using SupportService.Application.UseCases.CreateChat;
using SupportService.Application.UseCases.GetAllChats;
using SupportService.Application.UseCases.GetChatMessages;
using SupportService.Application.UseCases.GetUserChats;
using SupportService.Application.UseCases.SendMessage;

namespace SupportService.API.Controllers;

//Will be removed later
//Now it is used for use cases tests without signalR
[Route("api/[controller]")]
[ApiController]
public class TestController : ControllerBase
{
    private readonly IMediator _mediator;

    public TestController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetUserChats([FromQuery] GetUserChatsRequest request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request, cancellationToken);

        return Ok(result);
    }

    [HttpGet]
    [Route("all")]
    public async Task<IActionResult> GetAllChats(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetAllChatsRequest(), cancellationToken);

        return Ok(result);
    }

    [HttpGet]
    [Route("chat")]
    public async Task<IActionResult> GetChatMessages([FromQuery] GetChatMessagesRequest request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request, cancellationToken);

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateChat(CreateChatRequest request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request, cancellationToken);

        return Ok(result);
    }

    [HttpPatch]
    public async Task<IActionResult> UpdateChat(CloseChatRequest request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request, cancellationToken);

        return Ok(result);
    }

    [HttpPost]
    [Route("Message")]
    public async Task<IActionResult> SendMessage(SendMessageRequest request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request, cancellationToken);

        return Ok(result);
    }
}
