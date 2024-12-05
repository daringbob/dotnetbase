using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace src.Models
{
    public class MessageBox : IAuditableEntity
    {
        [Key]
        public int Id { get; set; }


        [ForeignKey("LastMessage")]
        public int? LastMessageId { get; set; }
        public Messages? LastMessage { get; set; }

        [ForeignKey("Candidate")]
        public int CandidateId { get; set; }
        public User? Candidate { get; set; }

        [ForeignKey("Recruiter")]
        public int RecruiterId { get; set; }
        public User? Recruiter { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
    }


}
