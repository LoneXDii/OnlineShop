using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace SupportService.API.Hubs;

//[Authorize]
public class ChatHub : Hub
{
    public async Task SendMessageAsync(string message)
    {
        await Clients.All.SendAsync("RecieveMessage", $"{Context.ConnectionId}: {message}");
    }
}
