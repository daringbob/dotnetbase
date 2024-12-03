using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace src.Dtos.Chat
{
    public class SendMessage
    {
        public int RecruiterId { get; set; } = 0;
        public int CandidateId { get; set; } = 0;
        public int SenderId { get; set; } = 0;
        public string Message { get; set; } = "";

    }
}