using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductsService.Application.DTO;
using ProductsService.Application.UseCases.ProductUseCases.Commands.AddProduct;

namespace ProductsService.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProductsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    //[Authorize(Policy = "admin")]
    public async Task<IActionResult> CreateProduct([FromForm] PostProductDTO product)
    {
        await _mediator.Send(new AddProductRequest(product));

        return Created();
    }
}
