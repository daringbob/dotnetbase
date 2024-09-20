using System.ComponentModel.DataAnnotations;
using src.Models;

namespace src.Dtos.Auth
{
    public class RegisterDto
    {
        public required string UserName { get; set; }

        public required string Password { get; set; }

        public int UserId { get; set; }

        public int RoleId { get; set; } = 2;
        public string AccountType { get; set; } = "User";
    }
}
