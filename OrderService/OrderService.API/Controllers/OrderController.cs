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
using OrderService.Application.UseCases.PaymentUseCases.PayOrderUseCase;
using AutoMapper;

namespace OrderService.API.Controllers;

[Route("api/orders")]
[ApiController]
public class OrderController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public OrderController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrder(CancellationToken cancellationToken)
    {
        var userId = HttpContext.User.FindFirst("Id")?.Value;

        await _mediator.Send(new CreateOrderRequest(userId), cancellationToken);

        return NoContent();
    }

    [HttpGet]
    [Authorize]
    public async Task<ActionResult<PaginatedListModel<OrderDTO>>> GetOrders(CancellationToken cancellationToken,
        [FromQuery] PaginationDTO pagination)
    {
        var request = _mapper.Map<GetUserOrdersRequest>(pagination);
        request.UserId = HttpContext.User.FindFirst("Id")?.Value;

        var data = await _mediator.Send(request, cancellationToken);

        return Ok(data);
    }

    [HttpGet("all")]
    [Authorize(Policy = "admin")]
    public async Task<ActionResult<PaginatedListModel<OrderDTO>>> GetAllOrders(CancellationToken cancellationToken,
        [FromQuery] PaginationDTO pagination)
    {
        var request = _mapper.Map<GetAllOrdersRequest>(pagination);

        var data = await _mediator.Send(request, cancellationToken);

        return Ok(data);
    }

    [HttpGet("user/{userId:regex(^[[a-fA-F0-9]]{{24}}$)}")]
    [Authorize(Policy = "Admin")]
    public async Task<ActionResult<PaginatedListModel<OrderDTO>>> GetUserOrders(CancellationToken cancellationToken,
        [FromRoute] string userId,
        [FromQuery] PaginationDTO pagination)
    {
        var request = _mapper.Map<GetUserOrdersRequest>(pagination);
        request.UserId = userId;

        var data = await _mediator.Send(request, cancellationToken);

        return Ok(data);
    }

    [HttpGet("{orderId:regex(^[[a-fA-F0-9]]{{24}}$)}")]
    [Authorize]
    public async Task<ActionResult<OrderDTO>> GetOrder([FromRoute] string orderId, CancellationToken cancellationToken)
    {
        var userId = HttpContext.User.FindFirst("Id")?.Value;

        var data = await _mediator.Send(new GetOrderByIdRequest(orderId, userId), cancellationToken);

        return Ok(data);
    }

    [HttpGet("{orderId:regex(^[[a-fA-F0-9]]{{24}}$)}/admin")]
    [Authorize(Policy = "admin")]
    public async Task<ActionResult<OrderDTO>> GetOrderAdmin([FromRoute] string orderId, CancellationToken cancellationToken)
    {
        var data = await _mediator.Send(new GetOrderByIdRequest(orderId), cancellationToken);

        return Ok(data);
    }

    [HttpPut("{orderId:regex(^[[a-fA-F0-9]]{{24}}$)}/cancellation")]
    [Authorize]
    public async Task<IActionResult> CancelOrder([FromRoute] string orderId, CancellationToken cancellationToken)
    {
        var userId = HttpContext.User.FindFirst("Id")?.Value;

        await _mediator.Send(new CancelOrderRequest(orderId, userId), cancellationToken);

        return NoContent();
    }

    [HttpPut("{orderId:regex(^[[a-fA-F0-9]]{{24}}$)}/cancellation/admin")]
    [Authorize(Policy = "admin")]
    public async Task<IActionResult> CancelOrderAdmin([FromRoute] string orderId, CancellationToken cancellationToken)
    {
        await _mediator.Send(new CancelOrderRequest(orderId), cancellationToken);

        return NoContent();
    }

    [HttpPut("{orderId:regex(^[[a-fA-F0-9]]{{24}}$)}/confirmation/admin")]
    [Authorize(Policy = "admin")]
    public async Task<IActionResult> ConfirmOrder([FromRoute] string orderId, CancellationToken cancellationToken)
    {
        await _mediator.Send(new ConfirmOrderRequest(orderId), cancellationToken);

        return NoContent();
    }

    [HttpPut("{orderId:regex(^[[a-fA-F0-9]]{{24}}$)}/completion/admin")]
    [Authorize(Policy = "admin")]
    public async Task<IActionResult> CompleteOrder([FromRoute] string orderId, CancellationToken cancellationToken)
    {
        await _mediator.Send(new CompleteOrderRequest(orderId), cancellationToken);

        return NoContent();
    }

    [HttpGet]
    [Route("{orderId:regex(^[[a-fA-F0-9]]{{24}}$)}/pay")]
    [Authorize]
    public async Task<ActionResult<string>> Pay([FromRoute] string orderId, CancellationToken cancellationToken)
    {
        var userId = HttpContext.User.FindFirst("Id")?.Value;
        var stripeId = HttpContext.User.FindFirst("StripeId")?.Value;

        var stripeUrl = await _mediator.Send(new PayOrderRequest(orderId, userId, stripeId), cancellationToken);

        return Ok(stripeUrl);
    }
}
