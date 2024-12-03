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

         [ForeignKey("WorkingModel")]
        public int? WorkingModelId { get; set; }
        public Criterias? WorkingModel { get; set; }

        [ForeignKey("JobType")]
        public int? JobTypeId { get; set; }
        public Criterias? JobType { get; set; }
        [ForeignKey("Experience")]
        public int? ExperienceId { get; set; }
        public Criterias? Experience { get; set; }
        [ForeignKey("JobTitle")]
        public int? JobTitleId { get; set; }
        public Criterias? JobTitle { get; set; }

        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
    }

    
}
