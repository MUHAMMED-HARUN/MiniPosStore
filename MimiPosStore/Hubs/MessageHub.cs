using Microsoft.AspNetCore.SignalR;

namespace MimiPosStore.Hubs
{
    public class MessageHub:Hub
    {
        public async Task SendMessage(string targetUserId, string message)
        {
            // Clients.User يرسل الرسالة لكل الاتصالات الخاصة بالمستخدم المستهدف
            await Clients.All.SendAsync("ReceiveMessage", message);
        }
    }
}
