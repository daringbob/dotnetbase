using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace src.Dtos.Chat
{
    public class SendMessageDto
    {
        public int NewMessageId { get; set; }
        public int CandidateId { get; set; }
        public int RecruiterId { get; set; }
        public int SenderId { get; set; }
        public int? MessageBoxId { get; set; }
    }
}