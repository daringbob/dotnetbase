using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using src.Data;
using src.Dtos.Chat;
using src.Models;

namespace src.Hubs
{
    public class ChatHub : Hub
    {
        private readonly AppDbContext _context;

        public ChatHub(AppDbContext context)
        {
            _context = context;
        }

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
            Console.WriteLine(groupName);
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            await Clients.Group(groupName).SendAsync("JoinSpecifyBoxMessageResp",
                $"User {joinSpecifyBoxConnection.RecruiterId} and User {joinSpecifyBoxConnection.CandidateId} has joined the chat");
        }

        public async Task SendMessage(SendMessageDto sendMessage)
        {
            if (sendMessage == null || sendMessage.RecruiterId <= 0 || sendMessage.CandidateId <= 0)
            {
                throw new ArgumentException("Invalid message data.");
            }

            var groupName = $"Recruiter-{sendMessage.RecruiterId}-Candidate-{sendMessage.CandidateId}";
            var message = await _context.Messages.Include(m => m.Sender).Include(m => m.Recruiter).FirstOrDefaultAsync(m => m.Id == sendMessage.NewMessageId);
            var messageBox = await _context.MessageBox.Include(mb => mb.LastMessage).Include(mb => mb.Recruiter).Include(mb => mb.Candidate).FirstOrDefaultAsync(mb => mb.Id == sendMessage.MessageBoxId);
            Console.WriteLine(groupName);
            // Gửi tin nhắn đến toàn bộ group
            await Clients.Group(groupName).SendAsync("SendMessageResp", message);

            // Gửi tin nhắn trực tiếp cho người nhận
            var receiveId = sendMessage.SenderId == sendMessage.CandidateId ? sendMessage.RecruiterId : sendMessage.CandidateId;

            await Clients.Group(receiveId.ToString()).SendAsync("JoinChatReceiveMessage", messageBox);
        }
    }

}
