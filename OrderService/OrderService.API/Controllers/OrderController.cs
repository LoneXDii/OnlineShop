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
    [Authorize]
    public async Task<IActionResult> CreateOrder(CancellationToken cancellationToken)
    {
        var userId = HttpContext.User.FindFirst("Id")?.Value;

        await _mediator.Send(new CreateOrderRequest(userId), cancellationToken);

        return Ok();
    }

    [HttpGet]
    [Route("get")]
    [Authorize]
    public async Task<ActionResult<PaginatedListModel<OrderDTO>>> GetOrders(CancellationToken cancellationToken, 
        [FromQuery] int pageNo = 1, [FromQuery] int pageSize = 10)
    {
        var userId = HttpContext.User.FindFirst("Id")?.Value;

        var data = await _mediator.Send(new GetUserOrdersRequest(userId, pageNo, pageSize), cancellationToken);

        return Ok(data);
    }

    [HttpGet]
    [Route("get/userId={userId}")]
    [Authorize(Policy = "admin")]
    public async Task<ActionResult<PaginatedListModel<OrderDTO>>> GetOrders(CancellationToken cancellationToken, [FromQuery] string userId,
        [FromQuery] int pageNo = 1, [FromQuery] int pageSize = 10)
    {
        var data = await _mediator.Send(new GetUserOrdersRequest(userId, pageNo, pageSize), cancellationToken);

        return Ok(data);
    }

    [HttpGet]
    [Route("get/all")]
    [Authorize(Policy = "admin")]
    public async Task<ActionResult<PaginatedListModel<OrderDTO>>> GetAllOrders(CancellationToken cancellationToken, [FromQuery] int pageNo = 1, [FromQuery] int pageSize = 10)
    {
        var data = await _mediator.Send(new GetAllOrdersRequest(pageNo, pageSize), cancellationToken);

        return Ok(data);
    }

    [HttpGet]
    [Authorize]
    [Route("get/orderId={orderId}")]
    public async Task<ActionResult<OrderDTO>> GetOrder(string orderId, CancellationToken cancellationToken)
    {
        var userId = HttpContext.User.FindFirst("Id")?.Value;

        var data = await _mediator.Send(new GetOrderByIdRequest(orderId, userId), cancellationToken);

        return Ok(data);
    }

    [HttpGet]
    [Route("get/admin/orderId={orderId}")]
    [Authorize(Policy = "admin")]
    public async Task<ActionResult<OrderDTO>> GetOrderAdmin(string orderId, CancellationToken cancellationToken)
    {
        var data = await _mediator.Send(new GetOrderByIdRequest(orderId), cancellationToken);

        return Ok(data);
    }

    [HttpPut]
    [Route("cancel")]
    [Authorize]
    public async Task<IActionResult> CancelOrder(string orderId, CancellationToken cancellationToken)
    {
        var userId = HttpContext.User.FindFirst("Id")?.Value;

        await _mediator.Send(new CancelOrderRequest(orderId, userId), cancellationToken);

        return Ok();
    }

    [HttpPut]
    [Route("cancel/admin")]
    [Authorize(Policy = "admin")]
    public async Task<IActionResult> CancelOrderAdmin([FromQuery] string orderId, CancellationToken cancellationToken)
    {
        await _mediator.Send(new CancelOrderRequest(orderId), cancellationToken);

        return Ok();
    }

    [HttpPut]
    [Route("confirm")]
    [Authorize(Policy = "admin")]
    public async Task<IActionResult> ConfirmOrder([FromQuery] string orderId, CancellationToken cancellationToken)
    {
        await _mediator.Send(new ConfirmOrderRequest(orderId), cancellationToken);

        return Ok();
    }

    [HttpPut]
    [Route("complete")]
    [Authorize(Policy = "admin")]
    public async Task<IActionResult> CompleteOrder([FromQuery] string orderId, CancellationToken cancellationToken)
    {
        await _mediator.Send(new CompleteOrderRequest(orderId), cancellationToken);

        return Ok();
    }
}
