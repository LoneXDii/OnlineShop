using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderService.Application.UseCases.OrderUseCases.CreateOrderUseCase;

namespace OrderService.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OrderController : ControllerBase
{
	private readonly IMediator _mediator;

	public OrderController(IMediator mediator)
	{
		_mediator = mediator;
	}

	[HttpPost]
	//[Authorize]
	public async Task<IActionResult> CreateOrder()
	{
		//var userId = HttpContext.User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;
		await _mediator.Send(new CreateOrderRequest("test"));
		return Ok();
	}
}
