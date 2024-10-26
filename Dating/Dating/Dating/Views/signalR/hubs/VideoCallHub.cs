using Microsoft.AspNetCore.SignalR;

namespace Dating.Views.signalR.hubs
{
    public class VideoCallHub : Hub
    {
        public async Task SendOffer(string receiverId, string offer)
        {
            await Clients.User(receiverId).SendAsync("ReceiveOffer", offer, Context.UserIdentifier);
        }

        public async Task SendAnswer(string senderId, string answer)
        {
            await Clients.User(senderId).SendAsync("ReceiveAnswer", answer);
        }

        public async Task SendIceCandidate(string receiverId, string candidate)
        {
            await Clients.User(receiverId).SendAsync("ReceiveIceCandidate", candidate);
        }
    }
}
