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

[Route("api")]
[ApiController]
public class OrderController : ControllerBase
{
    private readonly IMediator _mediator;

    public OrderController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrder(CancellationToken cancellationToken)
    {
        var userId = HttpContext.User.FindFirst("Id")?.Value;

        await _mediator.Send(new CreateOrderRequest(userId), cancellationToken);

        return Ok();
    }

    [HttpGet("users/{userId}/orders")]
    [Authorize]
    public async Task<ActionResult<PaginatedListModel<OrderDTO>>> GetOrders(CancellationToken cancellationToken,
        [FromRoute] string userId,
        [FromQuery] PaginationDTO pagination)
    {
        var data = await _mediator.Send(new GetUserOrdersRequest(userId, pagination.PageNo, pagination.PageSize), cancellationToken);

        return Ok(data);
    }

    [HttpGet("orders")]
    [Authorize(Policy = "admin")]
    public async Task<ActionResult<PaginatedListModel<OrderDTO>>> GetAllOrders(CancellationToken cancellationToken,
        [FromQuery] PaginationDTO pagination)
    {
        var data = await _mediator.Send(new GetAllOrdersRequest(pagination.PageNo, pagination.PageSize), cancellationToken);

        return Ok(data);
    }

    [HttpGet("orders/{orderId}")]
    [Authorize]
    public async Task<ActionResult<OrderDTO>> GetOrder([FromRoute] OrderIdDTO orderId, CancellationToken cancellationToken)
    {
        var userId = HttpContext.User.FindFirst("Id")?.Value;

        var data = await _mediator.Send(new GetOrderByIdRequest(orderId.OrderId, userId), cancellationToken);

        return Ok(data);
    }

    [HttpGet("admin/orders/{orderId}")]
    [Authorize(Policy = "admin")]
    public async Task<ActionResult<OrderDTO>> GetOrderAdmin([FromRoute] OrderIdDTO orderId, CancellationToken cancellationToken)
    {
        var data = await _mediator.Send(new GetOrderByIdRequest(orderId.OrderId), cancellationToken);

        return Ok(data);
    }

    [HttpPut("orders/{orderId}/cancellation")]
    [Authorize]
    public async Task<IActionResult> CancelOrder([FromRoute] OrderIdDTO orderId, CancellationToken cancellationToken)
    {
        var userId = HttpContext.User.FindFirst("Id")?.Value;

        await _mediator.Send(new CancelOrderRequest(orderId.OrderId, userId), cancellationToken);

        return Ok();
    }

    [HttpPut("admin/orders/{orderId}/cancellation")]
    [Authorize(Policy = "admin")]
    public async Task<IActionResult> CancelOrderAdmin([FromRoute] OrderIdDTO orderId, CancellationToken cancellationToken)
    {
        await _mediator.Send(new CancelOrderRequest(orderId.OrderId), cancellationToken);

        return Ok();
    }

    [HttpPut("admin/orders/{orderId}/confirmation")]
    [Authorize(Policy = "admin")]
    public async Task<IActionResult> ConfirmOrder([FromRoute] OrderIdDTO orderId, CancellationToken cancellationToken)
    {
        await _mediator.Send(new ConfirmOrderRequest(orderId.OrderId), cancellationToken);

        return Ok();
    }

    [HttpPut("admin/orders/{orderId}/completion")]
    [Authorize(Policy = "admin")]
    public async Task<IActionResult> CompleteOrder([FromRoute] OrderIdDTO orderId, CancellationToken cancellationToken)
    {
        await _mediator.Send(new CompleteOrderRequest(orderId.OrderId), cancellationToken);

        return Ok();
    }
}
