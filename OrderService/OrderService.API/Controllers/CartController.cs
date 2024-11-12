using MediatR;
using Microsoft.AspNetCore.Mvc;
using OrderService.Application.DTO;
using OrderService.Application.UseCases.CartUseCases.AddProductToCartUseCase;
using OrderService.Application.UseCases.CartUseCases.GetCartUseCase;

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
	//[Authorize]
	public async Task<ActionResult<CartDTO>> GetCart()
	{
		var cart = await _mediator.Send(new GetCartRequest()); 
		return Ok(cart);
	}

	[HttpPost]
	//[Authorize]
	public async Task<IActionResult> AddToCart(CartProductDTO product)
	{
		await _mediator.Send(new AddProductToCartRequest(product));
		return Ok();
	}
}
