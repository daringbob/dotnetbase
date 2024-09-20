using System.Security.Claims;
using src.Dtos.Auth;
using src.Models;

namespace src.Repositories.Auth
{
    public interface IAuthRepository
    {
        Task<Account?> GetAccountByUserName(string username);
        Task<User?> GetUserById(int userId);
        Task<List<Permission?>> GetPermissionsOfUser(int userId);
        bool ValidatePassword(string reqPassword, string dbPassword);
        Task<bool> IsUsernameTaken(string username);
        Task<bool> RegisterAsync(RegisterDto registerDto);
        Task<bool> ChangePassword(Account account, string newPassword);
        Token? GenerateToken(string userName, string? role);
        Token? GenerateRefreshToken(string userName, string? role);
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
        int AddUserRefreshTokens(UserRefreshToken user);
        UserRefreshToken? GetSavedRefreshTokens(string username, string refreshtoken);
        void RevokeToken(string username, string refreshtoken);
    }
}
