using src.Models;

namespace src.Dtos.Auth
{
    public class LoginResDto
    {
        public Token? Token { get; set; }
        public required AccountDto Account { get; set; }
        public User? User { get; set; }
        public List<Permission?>? Permission { get; set; }
    }
}
