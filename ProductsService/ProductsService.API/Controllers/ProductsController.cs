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
    public async Task<IActionResult> CreateProduct([FromForm] PostProductDTO product, CancellationToken cancellationToken)
    {
        await _mediator.Send(new AddProductRequest(product), cancellationToken);

        return NoContent();
    }

    [HttpGet]
    public async Task<ActionResult<PaginatedListModel<ProductDTO>>> GetProductsListProducts(
        CancellationToken cancellationToken,
        [FromQuery] double? maxPrice,
        [FromQuery] double? minPrice,
        [FromQuery] int? categoryId,
        [FromQuery] Dictionary<string, string>? attributes,
        [FromQuery] int pageNo = 1,
        [FromQuery] int pageSize = 10)
    {
        var keysToIgnore = new HashSet<string> { "maxPrice", "minPrice", "categoryId", "pageNo", "pageSize" };

        attributes = attributes
            ?.Where(kv => !keysToIgnore.Contains(kv.Key))
            .ToDictionary(kv => kv.Key, kv => kv.Value);

        var requestDto = new ListProductsRequestDTO
        {
            PageNo = pageNo,
            PageSize = pageSize,
            MaxPrice = maxPrice,
            MinPrice = minPrice,
            CategoryId = categoryId
        };

        var result = await _mediator.Send(new ListProductsWithPaginationRequest(requestDto, attributes), cancellationToken);

        return Ok(result);
    }

    [HttpPut]
    //[Authorize(Policy = "admin")]
    public async Task<IActionResult> UpdateProduct([FromForm] UpdateProductDTO productDTO,
        CancellationToken cancellationToken)
    {
        await _mediator.Send(new UpdateProductRequest(productDTO), cancellationToken);

        return Ok();
    }

    [HttpPost]
    [Route("/attrubite")]
    //[Authorize(Policy = "admin")]
    public async Task<IActionResult> AddProductAttribute([FromBody] AddProductAttributeDTO productAttribute, 
        CancellationToken cancellationToken)
    {
        await _mediator.Send(new AddAttributeToProductRequest(productAttribute), cancellationToken);

        return Ok();
    }

    [HttpDelete]
    [Route("/attribute")]
    //[Authorize(Policy = "admin")]
    public async Task<IActionResult> DeleteProductAttribute([FromQuery] DeleteAttributeFromProductRequest request,
        CancellationToken cancellationToken)
    {
        await _mediator.Send(request, cancellationToken);

        return Ok();
    }
}
