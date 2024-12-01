using System.ComponentModel.DataAnnotations.Schema;

namespace src.Models
{
    public class Users : IAuditableEntity
    {
        public int Id { get; set; }
       public string? Gender { get; set; }
       public string ? Email { get; set; }

        public required string  PhoneNumber { get; set; }

        public required string Name { get; set; }

        public  string? About { get; set; }

        public  string? WorkingHours { get; set; }

        public string UserType { get; set; } = "Candidate";

        public DateTime LastAccessTime { get; set; }

        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
    }
}
