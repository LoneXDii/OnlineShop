using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductsService.Application.DTO;
using ProductsService.Application.UseCases.CategoryUseCases.Commands.AddAttribute;
using ProductsService.Application.UseCases.CategoryUseCases.Commands.AddCategory;
using ProductsService.Application.UseCases.CategoryUseCases.Commands.UpdateCategory;
using ProductsService.Application.UseCases.CategoryUseCases.Queries.GetAllCategories;
using ProductsService.Application.UseCases.CategoryUseCases.Queries.GetCategoryAttributes;
using ProductsService.Application.UseCases.CategoryUseCases.Queries.GetUniqueCategoryAttributesValues;

namespace ProductsService.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoriesController : ControllerBase
{
    private readonly IMediator _mediator;

    public CategoriesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<List<CategoryDTO>>> GetCategories(CancellationToken cancellationToken)
    {
        var categories = await _mediator.Send(new GetAllCategoriesReguest(), cancellationToken);

        return Ok(categories);
    }

    [HttpPost]
    //[Authorize(Policy = "admin")]
    public async Task<IActionResult> AddCategory([FromBody] AddCategoryRequest request,CancellationToken cancellationToken)
    {
        await _mediator.Send(request, cancellationToken);

        return Ok();
    }

    [HttpPut]
    //[Authorize(Policy = "admin")]
    public async Task<IActionResult> UpdateCategory([FromBody] UpdateCategoryRequest request, CancellationToken cancellationToken)
    {
        await _mediator.Send(request, cancellationToken);

        return Ok();
    }

    [HttpGet]
    [Route("attributes")]
    public async Task<ActionResult<List<CategoryDTO>>> GetCategoryAttributes([FromQuery] GetCategoryAttributesRequest request,
        CancellationToken cancellationToken)
    {
        var attributes = await _mediator.Send(request, cancellationToken);

        return Ok(attributes);
    }

    [HttpPost]
    [Route("attributes")]
    //[Authorize(Policy = "admin")]
    public async Task<IActionResult> AddCategoryAttribute([FromBody] AddAttributeRequest request, CancellationToken cancellationToken)
    {
        await _mediator.Send(request, cancellationToken);

        return Ok();
    }

    [HttpGet]
    [Route("attributes/values")]
    public async Task<ActionResult<List<CategoryAttributesValuesDTO>>> GetCategoryAttributesValues(
        [FromQuery] GetUniqueCategoryAttributesValuesRequest request,
        CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(request, cancellationToken);

        return Ok(response);
    }
}
