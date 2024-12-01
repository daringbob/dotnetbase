using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace src.Models
{
    public class Job : IAuditableEntity
    {
        [Key]
        public int Id { get; set; }
        public required string  Locations { get; set; }

        public required double MinSalary { get; set; }
        public required double MaxSalary { get; set; }
        public int ApplicantCount { get; set; }

        public required string Description { get; set; }

        public required string Requirement { get; set; }

        
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
    }

    
}
