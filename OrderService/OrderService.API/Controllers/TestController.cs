using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderService.Domain.Abstractions;
using OrderService.Domain.Entities;

namespace OrderService.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TestController : ControllerBase
{
	private readonly IDbService _dbService;

	public TestController(IDbService dbService)
	{
		_dbService = dbService;
	}

	[HttpPost]
	public async Task<IActionResult> Post(Order order)
	{
		await _dbService.CreateOrderAsync(order);
		return CreatedAtAction(nameof(Post), new {id = order.Id}, order);
	}

	[HttpPut]
	[Route("/{orderId}")]
	public async Task<IActionResult> AddToOrder(Product product, string orderId)
	{
		await _dbService.AddProductToOrderAsync(orderId, product);
		return Ok();
	}

	[HttpDelete]
	public async Task<IActionResult> DeleteFromOrder(string orderId, int productId)
	{
		await _dbService.DeleteProductFromOrderAsync(orderId, productId);
		return Ok();
	}
}
