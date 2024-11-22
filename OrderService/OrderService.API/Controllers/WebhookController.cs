using MediatR;
using Microsoft.AspNetCore.Mvc;
using OrderService.Application.UseCases.PaymentUseCases.ConfirmPaymentUseCase;

namespace OrderService.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class WebhookController : ControllerBase
{
    private readonly IMediator _mediator;

    public WebhookController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [Route("stripe")]
    public async Task<IActionResult> StripeWebhookHandler(CancellationToken cancellationToken)
    {
        var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync(cancellationToken);

        var signature = Request.Headers["Stripe-Signature"];

        await _mediator.Send(new ConfirmPaymentRequest(json, signature), cancellationToken);

        return Ok();
    }
}
