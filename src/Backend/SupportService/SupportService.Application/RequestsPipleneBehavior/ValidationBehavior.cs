using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace SupportService.Application.RequestsPipleneBehavior;

internal class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
    private readonly IValidator<TRequest> _validator;
    private readonly ILogger<ValidationBehavior<TRequest, TResponse>> _logger;

    public ValidationBehavior(IValidator<TRequest> validator, ILogger<ValidationBehavior<TRequest, TResponse>> logger)
    {
        _validator = validator;
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var validationResult = _validator.Validate(request);

        if (!validationResult.IsValid)
        {
            _logger.LogError($"Wrong request data");

            throw new ValidationException(validationResult.Errors);
        }

        return await next();
    }
}
