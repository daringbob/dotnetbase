using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace src.Models
{
    public class JobApplications : IAuditableEntity
    {
        [Key]
        public int Id { get; set; }
        public string? Description { get; set; }

        [ForeignKey("Jobs")]
        public int JobId { get; set; }
        public Job? Jobs { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }
        public Users? User { get; set; }
    
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
    }

    
}
