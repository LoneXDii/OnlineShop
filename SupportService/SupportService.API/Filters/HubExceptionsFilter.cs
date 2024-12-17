using Microsoft.AspNetCore.SignalR;
using SupportService.API.Hubs;

namespace SupportService.API.Filters;

public class HubExceptionsFilter : IHubFilter
{
	private readonly IHubContext<ChatHub> _hubContext;

    public HubExceptionsFilter(IHubContext<ChatHub> hubContext)
	{
		_hubContext = hubContext;
	}

	public async ValueTask<object?> InvokeMethodAsync(HubInvocationContext invocationContext, 
        Func<HubInvocationContext, ValueTask<object?>> next)
    {
        try
        {
            return await next(invocationContext);
        }
        catch (Exception ex)
        {
			await _hubContext.Clients
                .Client(invocationContext.Context.ConnectionId)
                .SendAsync("HandleError", ex.Message);

            throw new HubException(ex.Message);
		}
    }
}
