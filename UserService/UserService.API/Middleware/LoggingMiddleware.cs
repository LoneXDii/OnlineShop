namespace UserService.API.Middleware;

public class LoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<LoggingMiddleware> _logger;

    public LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        var protocol = context.Request.Protocol;
        var method = context.Request.Method;
        var path = context.Request.Path;
        var query = context.Request.QueryString;
        var userId = context.User.FindFirst("Id")?.Value ?? "Unauthorized";
        var request = $"{protocol} {method} {path}{query}";

        _logger.LogInformation($"Request: {request} started by user with id: {userId}");

        await _next(context);
    }
}
