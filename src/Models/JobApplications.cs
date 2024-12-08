using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace src.Models
{
    public class JobApplications : IAuditableEntity
    {
        [Key]
        public int Id { get; set; }
        public string? Description { get; set; }
        public string? Status { get; set; }

        [ForeignKey("Jobs")]
        public int JobId { get; set; }
        public Job? Jobs { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }
        public User? User { get; set; }

        [ForeignKey("CV")]
        public int? CVId { get; set; }
        public StoreRecord? CV { get; set; }

        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
    }


}
