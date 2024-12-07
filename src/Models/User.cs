using System.ComponentModel.DataAnnotations.Schema;

namespace src.Models
{
    public class User : IAuditableEntity
    {
        public int Id { get; set; }
        public string? Code { get; set; }
        public string? Gender { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public bool? IsActive { get; set; } = false;
        public bool? IsInputInformation { get; set; } = false;

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
        public string? About { get; set; }

        public string? WorkingHours { get; set; }

        public string UserType { get; set; } = "Candidate";

        public DateTime LastAccessTime { get; set; }

        public string? ImageUrl { get; set; }

        public string? CompanyName { get; set; }
        public string? CompanyAddress { get; set; }
        public string? CompanyLocation { get; set; }
        public string? CompanyDescription { get; set; }
        public string? CompanyEmail { get; set; }

        public string? GeoLocation { get; set; }
        
        [ForeignKey("Roles")]
        public int RoleId { get; set; } = 2;
        public Roles? Roles { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
    }
}
