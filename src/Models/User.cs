using System.ComponentModel.DataAnnotations.Schema;

namespace src.Models
{
    public class User : IAuditableEntity
    {
        public int Id { get; set; }
        public string? Code { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public bool? IsActive { get; set; } = false;
        public bool? IsInputInformation { get; set; } = false;
        public string? Gender { get; set; }

        public string? WorkingModel { get; set; }
        public string? JobType { get; set; }
        public string? Experience { get; set; }
        public string? JobTitle { get; set; }
        public string? UserType { get; set; }

        [ForeignKey("Roles")]
        public int RoleId { get; set; } = 2;
        public Roles? Roles { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
    }
}
