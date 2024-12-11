using MediatR;
using Microsoft.AspNetCore.Mvc;
using OrderService.Application.DTO;
using OrderService.Application.UseCases.CartUseCases.AddProductToCartUseCase;
using OrderService.Application.UseCases.CartUseCases.ClearCartUseCase;
using OrderService.Application.UseCases.CartUseCases.GetCartUseCase;
using OrderService.Application.UseCases.CartUseCases.ReduceItemQuantityInCartUseCase;
using OrderService.Application.UseCases.CartUseCases.RemoveItemFromCartUseCase;
using OrderService.Application.UseCases.CartUseCases.SetItemQuantityInCartUseCase;

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
    public async Task<ActionResult<CartDTO>> GetCart()
    {
        var cart = await _mediator.Send(new GetCartRequest());

        return Ok(cart);
    }

    [HttpPost("products")]
    public async Task<IActionResult> AddToCart([FromBody] CartProductDTO product)
    {
        await _mediator.Send(new AddProductToCartRequest(product));

        return NoContent();
    }

    [HttpPost("products/{productId:min(1)}/quantity")]
    public async Task<IActionResult> SetQuantity([FromRoute] int productId, [FromBody] QuantityDTO quantity)
    {
        await _mediator.Send(new SetItemQuantityInCartRequest(productId, quantity.Quantity));

        return NoContent();
    }

    [HttpPatch("products/{productId:min(1)}/quantity")]
    public async Task<IActionResult> ReduceQuantity([FromRoute] int productId, [FromBody] QuantityDTO quantity)
    {
        await _mediator.Send(new ReduceItemsInCartRequest(productId, quantity.Quantity));

        return NoContent();
    }

    [HttpDelete("products/{productId:min(1)}")]
    public async Task<IActionResult> RemoveItemFromCart([FromRoute] int productId)
    {
        await _mediator.Send(new RemoveItemFromCartRequest(productId));

        return NoContent();
    }

    [HttpDelete]
    public async Task<IActionResult> ClearCart()
    {
        await _mediator.Send(new ClearCartRequest());

        return NoContent();
    }
}
