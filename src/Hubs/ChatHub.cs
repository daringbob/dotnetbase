using Microsoft.AspNetCore.SignalR;
using src.Dtos.Chat;

namespace src.Hubs
{
    public class ChatHub : Hub
    {
        public async Task JoinChat(JoinChatConnection joinChatConnection)
        {
            if (joinChatConnection == null || joinChatConnection.UserId <= 0)
            {
                throw new ArgumentException("Invalid connection information.");
            }

            await Groups.AddToGroupAsync(Context.ConnectionId, joinChatConnection.UserId.ToString());
        }

        public async Task JoinSpecifyBoxMessage(JoinSpecifyBoxConnection joinSpecifyBoxConnection)
        {
            if (joinSpecifyBoxConnection == null || joinSpecifyBoxConnection.RecruiterId <= 0 || joinSpecifyBoxConnection.CandidateId <= 0)
            {
                throw new ArgumentException("Invalid connection information.");
            }

            var groupName = $"Recruiter-{joinSpecifyBoxConnection.RecruiterId}-Candidate-{joinSpecifyBoxConnection.CandidateId}";
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            await Clients.Group(groupName).SendAsync("JoinSpecifyBoxMessageResp",
                $"User {joinSpecifyBoxConnection.RecruiterId} and User {joinSpecifyBoxConnection.CandidateId} has joined the chat");
        }

        public async Task SendMessage(SendMessage sendMessage)
        {
            if (sendMessage == null || sendMessage.RecruiterId <= 0 || sendMessage.CandidateId <= 0 || string.IsNullOrWhiteSpace(sendMessage.Message))
            {
                throw new ArgumentException("Invalid message data.");
            }

            var groupName = $"Recruiter-{sendMessage.RecruiterId}-Candidate-{sendMessage.CandidateId}";

            // Gửi tin nhắn đến toàn bộ group
            await Clients.Group(groupName).SendAsync("SendMessageResp", new
            {
                sendMessage.SenderId,
                sendMessage.Message
            });

            // Gửi tin nhắn trực tiếp cho người nhận
            var receiveId = sendMessage.SenderId == sendMessage.CandidateId ? sendMessage.RecruiterId : sendMessage.CandidateId;
            await Clients.Group(receiveId.ToString()).SendAsync("SendMessageResp", new
            {
                sendMessage.SenderId,
                sendMessage.Message
            });
        }
    }

}
