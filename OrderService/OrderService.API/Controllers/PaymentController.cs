using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderService.Application.DTO;
using OrderService.Application.UseCases.PaymentUseCases.PayOrderUseCase;

namespace OrderService.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PaymentController : ControllerBase
{
    private readonly IMediator _mediator;

    public PaymentController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [Route("/api/orders/{orderId}/pay")]
    [Authorize]
    public async Task<ActionResult<string>> Pay(string orderId, CancellationToken cancellationToken)
    {
        var userId = HttpContext.User.FindFirst("Id")?.Value;
        var stribeId = HttpContext.User.FindFirst("StribeId")?.Value;

        var stribeUrl = await _mediator.Send(new PayOrderRequest(orderId, userId, stribeId), cancellationToken);

        return Ok(stribeUrl);
    }
}
