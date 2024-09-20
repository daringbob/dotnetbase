using src.Models;

namespace src.Dtos.Auth
{
    public class AccountDto
    {
        public int Id { get; set; }
        public string? UserName { get; set; }
        public bool IsActive { get; set; } = true;

        public string AccountType { get; set; } = "User";
        public int UserId { get; set; }
        public string? Role { get; set; } = "User";
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
    }
}
