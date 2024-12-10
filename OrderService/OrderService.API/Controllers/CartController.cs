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

    [HttpPost("products/{productId:int}/quantity")]
    public async Task<IActionResult> SetQuantity(int productId, [FromBody] int quantity)
    {
        await _mediator.Send(new SetItemQuantityInCartRequest(productId, quantity));

        return NoContent();
    }

    [HttpPatch("products/{productId:int}/quantity")]
    public async Task<IActionResult> ReduceQuantity(int productId, [FromBody] int quantity)
    {
        await _mediator.Send(new ReduceItemsInCartRequest(productId, quantity));

        return NoContent();
    }

    [HttpDelete("products/{productId:int}")]
    public async Task<IActionResult> RemoveItemFromCart(int productId)
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
