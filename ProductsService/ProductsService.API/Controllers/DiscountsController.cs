using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductsService.Application.UseCases.CategoryUseCases.Commands.DeleteCategory;
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

    [HttpDelete("{discountId:min(1)}")]
    [Authorize(Policy = "admin")]
    public async Task<IActionResult> DeleteDiscount([FromRoute] int discountId, CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeleteCategoryRequest(discountId), cancellationToken);

        return NoContent();
    }
}
