using Microsoft.AspNetCore.SignalR;
using SupportService.API.Hubs;

namespace SupportService.API.Filters;

public class HubExceptionsFilter : IHubFilter
{
	private readonly IHubContext<ChatHub> _hubContext;
    private readonly ILogger<HubExceptionsFilter> _logger;

    public HubExceptionsFilter(IHubContext<ChatHub> hubContext, ILogger<HubExceptionsFilter> logger)
	{
		_hubContext = hubContext;
        _logger = logger;
	}

	public async ValueTask<object?> InvokeMethodAsync(HubInvocationContext invocationContext, 
        Func<HubInvocationContext, ValueTask<object?>> next)
    {
        try
        {
            _logger.LogInformation($"Processing ws request: {invocationContext.HubMethod.Name}");

            return await next(invocationContext);
        }
        catch (Exception ex)
        {
			await _hubContext.Clients
                .Client(invocationContext.Context.ConnectionId)
                .SendAsync("HandleError", ex.Message);

            _logger.LogError(ex, ex.Message);

            throw new HubException(ex.Message);
		}
    }
}
