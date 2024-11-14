using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderService.Application.DTO;
using OrderService.Application.UseCases.OrderUseCases.CancelOrderUseCase;
using OrderService.Application.UseCases.OrderUseCases.CompleteOrderUseCase;
using OrderService.Application.UseCases.OrderUseCases.ConfirmOrderUseCase;
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
	[Route("create")]
	//[Authorize]
	public async Task<IActionResult> CreateOrder()
	{
		//var userId = HttpContext.User.FindFirst("Id")?.Value;
		await _mediator.Send(new CreateOrderRequest("2"));
		return Ok();
	}

	[HttpGet]
	[Route("get")]
	[Authorize]
	public async Task<ActionResult<PaginatedListModel<GetOrderDTO>>> GetOrders(int pageNo = 1, int pageSize = 10)
	{
		var userId = HttpContext.User.FindFirst("Id")?.Value;
		var data = await _mediator.Send(new GetUserOrdersRequest(userId, pageNo, pageSize));
		return Ok(data);
	}

	[HttpGet]
	[Route("get/userId={userId}")]
	//[Authorize(Policy = "admin")]
	public async Task<ActionResult<PaginatedListModel<GetOrderDTO>>> GetOrders(string userId, int pageNo = 1, int pageSize = 10)
	{
		var data = await _mediator.Send(new GetUserOrdersRequest(userId, pageNo, pageSize));
		return Ok(data);
	}

	[HttpGet]
	[Route("getall")]
	//[Authorize(Policy = "admin")]
	public async Task<ActionResult<PaginatedListModel<GetOrderDTO>>> GetAllOrders(int pageNo = 1, int pageSize = 10)
	{
		var data = await _mediator.Send(new GetAllOrdersRequest(pageNo, pageSize));
		return Ok(data);
	}

	[HttpGet]
	[Authorize]
	[Route("get/orderId={orderId}")]
	public async Task<ActionResult<GetOrderDTO>> GetOrder(string orderId)
	{
		var userId = HttpContext.User.FindFirst("Id")?.Value;
		var data = await _mediator.Send(new GetOrderByIdRequest(orderId, userId));
		return Ok(data);
	}

	[HttpGet]
	[Route("get/admin/orderId={orderId}")]
	//[Authorize(Policy = "admin")]
	public async Task<ActionResult<GetOrderDTO>> GetOrderAdmin(string orderId)
	{
		var data = await _mediator.Send(new GetOrderByIdRequest(orderId));
		return Ok(data);
	}

	[HttpPut]
	[Route("cancel")]
	[Authorize]
	public async Task<IActionResult> CancelOrder(string orderId)
	{
		var userId = HttpContext.User.FindFirst("Id")?.Value;
		await _mediator.Send(new CancelOrderRequest(orderId, userId));
		return Ok();
	}

	[HttpPut]
	[Route("cancel/admin")]
	//[Authorize(Policy = "admin")]
	public async Task<IActionResult> CancelOrderAdmin(string orderId)
	{
		await _mediator.Send(new CancelOrderRequest(orderId));
		return Ok();
	}

	[HttpPut]
	[Route("confirm")]
	//[Authorize(Policy = "admin")]
	public async Task<IActionResult> ConfirmOrder(string orderId)
	{
		await _mediator.Send(new ConfirmOrderRequest(orderId));
		return Ok();
	}

	[HttpPut]
	[Route("complete")]
	//[Authorize(Policy = "admin")]
	public async Task<IActionResult> CompleteOrder(string orderId)
	{
		await _mediator.Send(new CompleteOrderRequest(orderId));
		return Ok();
	}
}
