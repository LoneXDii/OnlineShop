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

[Route("api/[controller]")]
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

    [HttpPost]
    [Route("add")]
    public async Task<IActionResult> AddToCart([FromBody] CartProductDTO product)
    {
        await _mediator.Send(new AddProductToCartRequest(product));

        return Ok();
    }

    [HttpPost]
    [Route("set")]
    public async Task<IActionResult> SetQuantity([FromBody] CartProductDTO product)
    {
        await _mediator.Send(new SetItemQuantityInCartRequest(product));

        return Ok();
    }

    [HttpPost]
    [Route("reduce")]
    public async Task<IActionResult> ReduceQuantity([FromBody] CartProductDTO product)
    {
        await _mediator.Send(new ReduceItemsInCartRequest(product));

        return Ok();
    }

    [HttpDelete]
    [Route("remove")]
    public async Task<IActionResult> RemoveItemFromCart([FromQuery] RemoveItemFromCartRequest request)
    {
        await _mediator.Send(request);

        return Ok();
    }

    [HttpDelete]
    [Route("clear")]
    public async Task<IActionResult> ClearCart()
    {
        await _mediator.Send(new ClearCartRequest());

        return Ok();
    }
}
