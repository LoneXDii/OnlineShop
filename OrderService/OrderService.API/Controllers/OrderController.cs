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
using OrderService.Application.Models;

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
    [Authorize]
    public async Task<IActionResult> CreateOrder(CancellationToken cancellationToken)
    {
        var userId = HttpContext.User.FindFirst("Id")?.Value;

        await _mediator.Send(new CreateOrderRequest(userId), cancellationToken);

        return Ok();
    }

    [HttpGet]
    [Route("get/user")]
    [Authorize]
    public async Task<ActionResult<PaginatedListModel<OrderDTO>>> GetOrders(CancellationToken cancellationToken, 
        [FromQuery] PaginationDTO pagination)
    {
        var userId = HttpContext.User.FindFirst("Id")?.Value;

        var data = await _mediator.Send(new GetUserOrdersRequest(userId, pagination.PageNo, pagination.PageSize), cancellationToken);

        return Ok(data);
    }

    [HttpGet]
    [Route("get/user/admin")]
    [Authorize(Policy = "admin")]
    public async Task<ActionResult<PaginatedListModel<OrderDTO>>> GetOrders(CancellationToken cancellationToken, 
        [FromQuery] string userId,
        [FromQuery] PaginationDTO pagination)
    {
        var data = await _mediator.Send(new GetUserOrdersRequest(userId, pagination.PageNo, pagination.PageSize), cancellationToken);

        return Ok(data);
    }

    [HttpGet]
    [Route("get/all")]
    [Authorize(Policy = "admin")]
    public async Task<ActionResult<PaginatedListModel<OrderDTO>>> GetAllOrders(CancellationToken cancellationToken, 
        [FromQuery] PaginationDTO pagination)
    {
        var data = await _mediator.Send(new GetAllOrdersRequest(pagination.PageNo, pagination.PageSize), cancellationToken);

        return Ok(data);
    }

    [HttpGet]
    [Authorize]
    [Route("get/id")]
    public async Task<ActionResult<OrderDTO>> GetOrder([FromQuery] OrderIdDTO orderId, CancellationToken cancellationToken)
    {
        var userId = HttpContext.User.FindFirst("Id")?.Value;

        var data = await _mediator.Send(new GetOrderByIdRequest(orderId.OrderId, userId), cancellationToken);

        return Ok(data);
    }

    [HttpGet]
    [Route("get/admin")]
    [Authorize(Policy = "admin")]
    public async Task<ActionResult<OrderDTO>> GetOrderAdmin([FromQuery] OrderIdDTO orderId, CancellationToken cancellationToken)
    {
        var data = await _mediator.Send(new GetOrderByIdRequest(orderId.OrderId), cancellationToken);

        return Ok(data);
    }

    [HttpPut]
    [Route("cancel")]
    [Authorize]
    public async Task<IActionResult> CancelOrder([FromQuery] OrderIdDTO orderId, CancellationToken cancellationToken)
    {
        var userId = HttpContext.User.FindFirst("Id")?.Value;

        await _mediator.Send(new CancelOrderRequest(orderId.OrderId, userId), cancellationToken);

        return Ok();
    }

    [HttpPut]
    [Route("cancel/admin")]
    [Authorize(Policy = "admin")]
    public async Task<IActionResult> CancelOrderAdmin([FromQuery] OrderIdDTO orderId, CancellationToken cancellationToken)
    {
        await _mediator.Send(new CancelOrderRequest(orderId.OrderId), cancellationToken);

        return Ok();
    }

    [HttpPut]
    [Route("confirm")]
    [Authorize(Policy = "admin")]
    public async Task<IActionResult> ConfirmOrder([FromQuery] OrderIdDTO orderId, CancellationToken cancellationToken)
    {
        await _mediator.Send(new ConfirmOrderRequest(orderId.OrderId), cancellationToken);

        return Ok();
    }

    [HttpPut]
    [Route("complete")]
    [Authorize(Policy = "admin")]
    public async Task<IActionResult> CompleteOrder([FromQuery] OrderIdDTO orderId, CancellationToken cancellationToken)
    {
        await _mediator.Send(new CompleteOrderRequest(orderId.OrderId), cancellationToken);

        return Ok();
    }
}
