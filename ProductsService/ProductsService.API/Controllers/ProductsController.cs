using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductsService.Application.DTO;
using ProductsService.Application.UseCases.ProductUseCases.Commands.AddProduct;
using ProductsService.Application.UseCases.ProductUseCases.Queries.ListProducts;
using ProductsService.Application.Models;
using ProductsService.Application.UseCases.ProductUseCases.Commands.AddAttributeToProduct;
using ProductsService.Application.UseCases.ProductUseCases.Commands.DeleteAttributeFromProduct;
using ProductsService.Application.UseCases.ProductUseCases.Commands.UpdateProduct;
using ProductsService.Application.UseCases.ProductUseCases.Commands.UpdateProductAttribute;
using ProductsService.Application.UseCases.ProductUseCases.Commands.DeleteProduct;
using ProductsService.Application.UseCases.ProductUseCases.Queries.GetProductById;

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

    [HttpGet]
    public async Task<ActionResult<PaginatedListModel<ProductDTO>>> GetProducts(
        [FromQuery] ListProductsWithPaginationRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request, cancellationToken);

        return Ok(result);
    }

    [HttpGet]
    [Route("Product")]
    public async Task<ActionResult<ProductDTO>> GetProductById([FromQuery] GetProductByIdRequest request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request, cancellationToken);

        return Ok(result);
    }

    [HttpPost]
    [Authorize(Policy = "admin")]
    public async Task<IActionResult> CreateProduct([FromForm] AddProductRequest request, CancellationToken cancellationToken)
    {
        await _mediator.Send(request, cancellationToken);

        return NoContent();
    }

    [HttpPut]
    [Authorize(Policy = "admin")]
    public async Task<IActionResult> UpdateProduct([FromForm] UpdateProductRequest request, CancellationToken cancellationToken)
    {
        await _mediator.Send(request, cancellationToken);

        return Ok();
    }

    [HttpDelete]
    [Authorize(Policy = "admin")]
    public async Task<IActionResult> DeleteProduct([FromQuery] DeleteProductRequest request, CancellationToken cancellationToken)
    {
        await _mediator.Send(request, cancellationToken);

        return Ok();
    }

    [HttpPost]
    [Route("attrubite")]
    //[Authorize(Policy = "admin")]
    public async Task<IActionResult> AddProductAttribute([FromBody] AddAttributeToProductRequest request,
        CancellationToken cancellationToken)
    {
        await _mediator.Send(request, cancellationToken);

        return Ok();
    }

    [HttpPut]
    [Route("attribute")]
    [Authorize(Policy = "admin")]
    public async Task<IActionResult> UpdateProductAttribute([FromBody] UpdateProductAttributeRequest request,
        CancellationToken cancellationToken)
    {
        await _mediator.Send(request, cancellationToken);

        return Ok();
    }

    [HttpDelete]
    [Route("attribute")]
    [Authorize(Policy = "admin")]
    public async Task<IActionResult> DeleteProductAttribute([FromQuery] DeleteAttributeFromProductRequest request,
        CancellationToken cancellationToken)
    {
        await _mediator.Send(request, cancellationToken);

        return Ok();
    }
}
