using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductsService.Application.DTO;
using ProductsService.Application.UseCases.CategoryUseCases.Commands.AddAttribute;
using ProductsService.Application.UseCases.CategoryUseCases.Commands.AddCategory;
using ProductsService.Application.UseCases.CategoryUseCases.Commands.DeleteCategory;
using ProductsService.Application.UseCases.CategoryUseCases.Commands.UpdateCategory;
using ProductsService.Application.UseCases.CategoryUseCases.Queries.GetAllCategories;
using ProductsService.Application.UseCases.CategoryUseCases.Queries.GetCategoryAttributes;
using ProductsService.Application.UseCases.CategoryUseCases.Queries.GetCategoryById;
using ProductsService.Application.UseCases.CategoryUseCases.Queries.GetUniqueCategoryAttributesValues;

namespace ProductsService.API.Controllers;

[Route("api/categories")]
[ApiController]
public class CategoriesController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public CategoriesController(IMediator mediator, IMapper maper)
    {
        _mediator = mediator;
        _mapper = maper;
    }

    [HttpGet]
    public async Task<ActionResult<List<ResponseCategoryDTO>>> GetCategories(CancellationToken cancellationToken)
    {
        var categories = await _mediator.Send(new GetAllCategoriesReguest(), cancellationToken);

        return Ok(categories);
    }

    [HttpGet("{CategoryId:min(1)}")]
    public async Task<ActionResult<ResponseCategoryDTO>> GetCategory([FromRoute] GetCategoryByIdRequest request, 
        CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(request, cancellationToken);

        return Ok(response);
    }

    [HttpPost]
    [Authorize(Policy = "admin")]
    public async Task<IActionResult> AddCategory([FromForm] AddCategoryDTO category, CancellationToken cancellationToken)
    {
        var request = _mapper.Map<AddCategoryRequest>(category);

        await _mediator.Send(request, cancellationToken);

        return NoContent();
    }

    [HttpPut("{id:min(1)}")]
    [Authorize(Policy = "admin")]
    public async Task<IActionResult> UpdateCategory([FromRoute] int id, [FromForm] UpdateCategoryDTO categoryDTO, 
        CancellationToken cancellationToken)
    {
        var request = _mapper.Map<UpdateCategoryRequest>(categoryDTO);
        request.CategoryId = id;

        await _mediator.Send(request, cancellationToken);

        return NoContent();
    }

    [HttpDelete("{CategoryId:min(1)}")]
    [Authorize(Policy = "admin")]
    public async Task<IActionResult> DeleteCategory([FromRoute] DeleteCategoryRequest request, CancellationToken cancellationToken)
    {
        await _mediator.Send(request, cancellationToken);

        return NoContent();
    }

    [HttpGet("{CategoryId:min(1)}/attributes")]
    public async Task<ActionResult<List<ResponseCategoryDTO>>> GetCategoryAttributes([FromRoute] GetCategoryAttributesRequest request,
        CancellationToken cancellationToken)
    {
        var attributes = await _mediator.Send(request, cancellationToken);

        return Ok(attributes);
    }

    [HttpPost("{categoryId:min(1)}/attributes")]
    [Authorize(Policy = "admin")]
    public async Task<IActionResult> AddCategoryAttribute([FromRoute] int categoryId, [FromBody] CategoryNameDTO categoryName,
        CancellationToken cancellationToken)
    {
        await _mediator.Send(new AddAttributeRequest(categoryId, categoryName.Name), cancellationToken);

        return NoContent();
    }

    [HttpGet("{CategoryId:min(1)}/attributes/values")]
    public async Task<ActionResult<List<CategoryAttributesValuesDTO>>> GetCategoryAttributesValues(
        [FromRoute] GetUniqueCategoryAttributesValuesRequest request,
        CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(request, cancellationToken);

        return Ok(response);
    }
}
