using MediatR;
using Microsoft.AspNetCore.Mvc;
using OrderService.Application.DTO;
using OrderService.Application.UseCases.CartUseCases.AddProductToCartUseCase;
using OrderService.Application.UseCases.CartUseCases.ClearCartUseCase;
using OrderService.Application.UseCases.CartUseCases.GetCartUseCase;
using OrderService.Application.UseCases.CartUseCases.ReduceItemQuantityInCartUseCase;
using OrderService.Application.UseCases.CartUseCases.RemoveItemFromCartUseCase;
using OrderService.Application.UseCases.CartUseCases.SetItemQuantityInCartUseCase;
using System.Threading;

namespace OrderService.API.Controllers;

[Route("api/cart")]
[ApiController]
public class CartController : ControllerBase
{
    private readonly IMediator _mediator;

    public CartController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<CartDTO>> GetCart(CancellationToken cancellationToken)
    {
        var cart = await _mediator.Send(new GetCartRequest(), cancellationToken);

        return Ok(cart);
    }

    [HttpPost("products")]
    public async Task<IActionResult> AddToCart([FromBody] CartProductDTO product, CancellationToken cancellationToken)
    {
        await _mediator.Send(new AddProductToCartRequest(product), cancellationToken);

        return NoContent();
    }

    [HttpPost("products/{productId:min(1)}/quantity")]
    public async Task<IActionResult> SetQuantity([FromRoute] int productId, [FromBody] QuantityDTO quantity, 
        CancellationToken cancellationToken)
    {
        await _mediator.Send(new SetItemQuantityInCartRequest(productId, quantity.Quantity), cancellationToken);

        return NoContent();
    }

    [HttpPatch("products/{productId:min(1)}/quantity")]
    public async Task<IActionResult> ReduceQuantity([FromRoute] int productId, [FromBody] QuantityDTO quantity, 
        CancellationToken cancellationToken)
    {
        await _mediator.Send(new ReduceItemsInCartRequest(productId, quantity.Quantity), cancellationToken);

        return NoContent();
    }

    [HttpDelete("products/{productId:min(1)}")]
    public async Task<IActionResult> RemoveItemFromCart([FromRoute] int productId, 
        CancellationToken cancellationToken)
    {
        await _mediator.Send(new RemoveItemFromCartRequest(productId), cancellationToken);

        return NoContent();
    }

    [HttpDelete]
    public async Task<IActionResult> ClearCart(CancellationToken cancellationToken)
    {
        await _mediator.Send(new ClearCartRequest(), cancellationToken);

        return NoContent();
    }
}
