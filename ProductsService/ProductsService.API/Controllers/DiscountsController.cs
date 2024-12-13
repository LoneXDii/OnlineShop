using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductsService.Application.UseCases.DiscountUseCases.Commands.AddDiscount;
using ProductsService.Application.UseCases.DiscountUseCases.Commands.DeleteDiscount;

namespace ProductsService.API.Controllers;

[Route("api/discounts")]
[ApiController]
public class DiscountsController : ControllerBase
{
    private readonly IMediator _mediator;

    public DiscountsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [Authorize(Policy = "admin")]
    public async Task<IActionResult> AddDiscount([FromBody] AddDiscountRequest request, CancellationToken cancellationToken)
    {
        await _mediator.Send(request, cancellationToken);

        return NoContent();
    }

    [HttpDelete]
    [Authorize(Policy = "admin")]
    public async Task<IActionResult> DeleteDiscount([FromQuery] DeleteDiscountRequest request, CancellationToken cancellationToken)
    {
        await _mediator.Send(request, cancellationToken);

        return NoContent();
    }
}
