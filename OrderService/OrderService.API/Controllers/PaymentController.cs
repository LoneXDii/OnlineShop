using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
	[Route("pay")]
	//[Authorize]
	public async Task<ActionResult<string>> Pay(string orderId)
	{
		//var userId = HttpContext.User.FindFirst("Id")?.Value;
		//var stribeId = HttpContext.User.FindFirst("StribeId")?.Value;
		var userId = "2";
		var stribeId = "cus_RDays6zmorNVMp";

		var stribeUrl = await _mediator.Send(new PayOrderRequest(orderId, userId, stribeId));

		return Ok(stribeUrl);
	}
}
