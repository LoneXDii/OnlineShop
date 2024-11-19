using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProductsService.Application.UseCases.ProductUseCases.Queries.Test;
using ProductsService.Domain.Entities;

namespace ProductsService.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TestController : ControllerBase
{
	private readonly IMediator _mediator;

	public TestController(IMediator mediator)
	{
		_mediator = mediator;
	}

	[HttpGet]
	public async Task<ActionResult<IEnumerable<Product>>> Test()
	{
		var products = await _mediator.Send(new TestRequest());

		return Ok(products);
	}
}
