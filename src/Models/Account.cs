using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace src.Models
{
    public class Account : IAuditableEntity
    {
        [Key]
        public int Id { get; set; }
        public required string UserName { get; set; }
        public required string Password { get; set; }
        public bool? IsActive { get; set; } = true;
        public string AccountType { get; set; } = "User";

        [ForeignKey("User")]
        public required int UserId { get; set; }

        public User? User { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
    }

    public static class AccountType
    {
        public const string User = "User";
    }
}
