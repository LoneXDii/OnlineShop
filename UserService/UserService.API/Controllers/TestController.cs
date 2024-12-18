using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UserService.DAL.Services.MessageBrocker.ProducerService;

namespace UserService.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TestController : ControllerBase
{
    private readonly IProducerService _producerService;

    public TestController(IProducerService producerService)
    {
        _producerService = producerService;
    }

    [HttpGet]
    public async Task<IActionResult> Produce(CancellationToken cancellationToken)
    {
        await _producerService.ProduceAsync(cancellationToken);

        return Ok("Event Sent!");
    }
}
