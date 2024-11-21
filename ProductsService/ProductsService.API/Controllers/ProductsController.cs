using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductsService.Application.DTO;
using ProductsService.Application.UseCases.ProductUseCases.Commands.AddProduct;
using ProductsService.Application.UseCases.ProductUseCases.Queries.ListProducts;
using ProductsService.Domain.Common.Models;

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

        return NoContent();
    }

    [HttpGet]
    public async Task<ActionResult<PaginatedListModel<ProductDTO>>> GetProductsListProducts(
        [FromQuery] double? priceLessThan,
        [FromQuery] double? priceGreaterThan,
        [FromQuery] int? categoryId,
        [FromQuery] Dictionary<string, string>? attributes,
        [FromQuery] int pageNo = 1,
        [FromQuery] int pageSize = 10)
    {
        var keysToIgnore = new HashSet<string> { "priceLessThan", "priceGreaterThan", "categoryId", "pageNo", "pageSize"};

        attributes = attributes
            ?.Where(kv => !keysToIgnore.Contains(kv.Key))
            .ToDictionary(kv => kv.Key, kv => kv.Value);

        var requestDto = new ListProductsRequestDTO
        {
            PageNo = pageNo,
            PageSize = pageSize,
            PriceLessThan = priceLessThan,
            PriceGreaterThan = priceGreaterThan,
            CategoryId = categoryId
        };

        var result = await _mediator.Send(new ListProductsWithPaginationRequest(requestDto, attributes));

        return Ok(result);
    }
}
