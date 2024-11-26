using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductsService.Application.DTO;
using ProductsService.Application.UseCases.CategoryUseCases.Queries;

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
}
