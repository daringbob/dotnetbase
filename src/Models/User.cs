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
        public bool? IsActive { get; set; } = true;

        [ForeignKey("Roles")]
        public int RoleId { get; set; } = 2;
        public Roles? Roles { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
    }
}
