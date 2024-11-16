using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using OrderService.Application.Exceptions;
using System.Text.Json;

namespace OrderService.API.Middleware;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IWebHostEnvironment _env;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IWebHostEnvironment env)
    {
        _next = next;
        _logger = logger;
        _env = env;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    public async Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        var problemDetails = new ProblemDetails
        {
            Title = "An error occurred while processing your request.",
            Status = ex switch
            {
                CartException => StatusCodes.Status400BadRequest,
                ValidationException => StatusCodes.Status400BadRequest,
                OrderException => StatusCodes.Status400BadRequest,
                NotInCartException => StatusCodes.Status404NotFound,
                PaginationException => StatusCodes.Status404NotFound,
                NotFoundException => StatusCodes.Status404NotFound,
                AccessDeniedException => StatusCodes.Status403Forbidden,
                _ => StatusCodes.Status500InternalServerError
            },
            Detail = ex.Message,
            Instance = context.Request.Path
        };

        if (_env.IsDevelopment())
        {
            problemDetails.Extensions.Add("stackTrace", ex.StackTrace);
        }

        _logger.LogError(ex, ex.Message);
        context.Response.ContentType = "application/problem+json";
        context.Response.StatusCode = problemDetails.Status.Value;

        await context.Response.WriteAsJsonAsync(problemDetails);
    }
}