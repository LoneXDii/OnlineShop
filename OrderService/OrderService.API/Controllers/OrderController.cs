using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderService.Application.DTO;
using OrderService.Application.UseCases.OrderUseCases.CreateOrderUseCase;
using OrderService.Application.UseCases.OrderUseCases.GetAllOrdersUseCase;
using OrderService.Application.UseCases.OrderUseCases.GetOrderByIdUseCase;
using OrderService.Application.UseCases.OrderUseCases.GetUserOrdersUseCase;
using OrderService.Domain.Common.Models;

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
		//var userId = HttpContext.User.FindFirst("Id")?.Value;
		await _mediator.Send(new CreateOrderRequest("2"));
		return Ok();
	}

	[HttpGet]
	[Authorize]
	public async Task<ActionResult<PaginatedListModel<GetOrderDTO>>> GetOrders(int pageNo = 1, int pageSize = 10)
	{
		var userId = HttpContext.User.FindFirst("Id")?.Value;
		var data = await _mediator.Send(new GetUserOrdersRequest(userId, pageNo, pageSize));
		return Ok(data);
	}

	[HttpGet]
	[Route("userId={userId}")]
	//[Authorize(Policy = "admin")]
	public async Task<ActionResult<PaginatedListModel<GetOrderDTO>>> GetOrders(string userId, int pageNo = 1, int pageSize = 10)
	{
		var data = await _mediator.Send(new GetUserOrdersRequest(userId, pageNo, pageSize));
		return Ok(data);
	}

	[HttpGet]
	[Route("all")]
	//[Authorize(Policy = "admin")]
	public async Task<ActionResult<PaginatedListModel<GetOrderDTO>>> GetAllOrders(int pageNo = 1, int pageSize = 10)
	{
		var data = await _mediator.Send(new GetAllOrdersRequest(pageNo, pageSize));
		return Ok(data);
	}

	[HttpGet]
	[Authorize]
	[Route("orderId={orderId}")]
	public async Task<ActionResult<GetOrderDTO>> GetOrder(string orderId)
	{
		var userId = HttpContext.User.FindFirst("Id")?.Value;
		var data = await _mediator.Send(new GetOrderByIdRequest(orderId, userId));
		return Ok(data);
	}

	[HttpGet]
	[Route("admin/orderId={orderId}")]
	//[Authorize(Policy = "admin")]
	public async Task<ActionResult<GetOrderDTO>> GetOrderAdmin(string orderId)
	{
		var data = await _mediator.Send(new GetOrderByIdRequest(orderId));
		return Ok(data);
	}
}
