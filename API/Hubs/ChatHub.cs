using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace API.Hubs
{
    [Authorize]
    public class ChatHub : Hub
    {
        private static string GroupName(Guid inventoryId) => $"inv:{inventoryId}";

        public async Task JoinInventory(Guid inventoryId)
            => await Groups.AddToGroupAsync(Context.ConnectionId, GroupName(inventoryId));

        public async Task LeaveInventory(Guid inventoryId)
            => await Groups.RemoveFromGroupAsync(Context.ConnectionId, GroupName(inventoryId));
    }
}
