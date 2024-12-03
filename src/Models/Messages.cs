using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace src.Models
{
    public class Messages : IAuditableEntity
    {
        [Key]
        public int Id { get; set; }

        public required string Message { get; set; }
        public string? IsSystemMessage { get; set; }

        [ForeignKey("Recruiter")]
        public int RecruiterId { get; set; }
        public User? Recruiter { get; set; }
        [ForeignKey("Candidate")]
        public int CandidateId { get; set; }
        public User? Candidate { get; set; }

        [ForeignKey("Sender")]
        public int SenderId { get; set; }
        public User? Sender { get; set; }

        [ForeignKey("Jobs")]
        public int? JobId { get; set; }
        public Job? Jobs { get; set; }

        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
    }


}
