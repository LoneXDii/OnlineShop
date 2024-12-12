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
using AutoMapper;

namespace ProductsService.API.Controllers;

[Route("api/products")]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public ProductsController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<PaginatedListModel<ResponseProductDTO>>> GetProducts(
        [FromQuery] ListProductsWithPaginationRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request, cancellationToken);

        return Ok(result);
    }

    [HttpGet("{ProductId:min(1)}")]
    public async Task<ActionResult<ResponseProductDTO>> GetProductById([FromRoute] GetProductByIdRequest request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request, cancellationToken);

        return Ok(result);
    }

    [HttpPost]
    [Authorize(Policy = "admin")]
    public async Task<IActionResult> CreateProduct([FromForm] AddProductDTO product, CancellationToken cancellationToken)
    {
        var request = _mapper.Map<AddProductRequest>(product);

        await _mediator.Send(request, cancellationToken);

        return NoContent();
    }

    [HttpPut("{productId:min(1)}")]
    [Authorize(Policy = "admin")]
    public async Task<IActionResult> UpdateProduct([FromRoute] int productId, [FromForm] UpdateProductDTO product, 
        CancellationToken cancellationToken)
    {
        var request = _mapper.Map<UpdateProductRequest>(product);
        request.Id = productId;

        await _mediator.Send(request, cancellationToken);

        return NoContent();
    }

    [HttpDelete("{ProductId:min(1)}")]
    [Authorize(Policy = "admin")]
    public async Task<IActionResult> DeleteProduct([FromRoute] DeleteProductRequest request, CancellationToken cancellationToken)
    {
        await _mediator.Send(request, cancellationToken);

        return NoContent();
    }

    [HttpPost("{ProductId:min(1)}/attributes/{AttributeId:min(1)}")]
    [Authorize(Policy = "admin")]
    public async Task<IActionResult> AddProductAttribute([FromRoute] AddAttributeToProductRequest request,
        CancellationToken cancellationToken)
    {
        await _mediator.Send(request, cancellationToken);

        return NoContent();
    }

    [HttpPut("{ProductId:min(1)}/attributes/{AttributeId:min(1)}")]
    //[Authorize(Policy = "admin")]
    public async Task<IActionResult> UpdateProductAttribute([FromRoute] RequestAttributeValueDTO productAttrubute, 
        [FromBody] UpdateAttributeDTO newAttribute, CancellationToken cancellationToken)
    {
        var request = _mapper.Map<UpdateProductAttributeRequest>(productAttrubute);
        request.NewAttributeId = newAttribute.AttributeId;

        await _mediator.Send(request, cancellationToken);

        return NoContent();
    }

    [HttpDelete("{ProductId:min(1)}/attributes/{AttributeId:min(1)}")]
    [Authorize(Policy = "admin")]
    public async Task<IActionResult> DeleteProductAttribute([FromRoute] DeleteAttributeFromProductRequest request,
        CancellationToken cancellationToken)
    {
        await _mediator.Send(request, cancellationToken);

        return NoContent();
    }
}
