using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Tokens;
using src.Data;
using src.Dtos.Auth;
using src.Extensions;
using src.Models;

namespace src.Repositories.Auth
{
    public class AuthRepository : IAuthRepository
    {
        private readonly IConfiguration _config;
        private readonly IMemoryCache _cache;
        private readonly AppDbContext _context;

        public AuthRepository(IConfiguration config, IMemoryCache cache, AppDbContext context)
        {
            _config = config;
            _cache = cache;
            _context = context;
        }

        public async Task<Account?> GetAccountByUserName(string username)
        {
            var account = await _context.Accounts.FirstOrDefaultAsync(a => a.UserName == username);

            if (account == null)
            {
                return null;
            }

            return account;
        }

        public async Task<User?> GetUserById(int userId)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
        }

        // New method to fetch permissions of a user based on their assigned role
        public async Task<List<Permission?>> GetPermissionsOfUser(int userId)

        {
            // Fetch the user with the associated role and permissions
            var user = await _context.Users
                                      .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null || user.Roles == null)
                return []; // Return empty if the user or role doesn't exist

            // Fetch the permissions associated with the user's role
            var permissions = user.Roles.RolePermissions
                                        .Select(rp => rp.Permission)
                                        .Where(p => p != null) // Ensure permissions are not null
                                        .ToList();

            return permissions;
        }

        public bool ValidatePassword(string reqPassword, string dbPassword)
        {
            string salt = _config.GetValue<string>("PasswordSalt") ?? "";

            return PasswordHasherExtensions.VerifyPassword(reqPassword, dbPassword, salt);
        }

        public async Task<bool> IsUsernameTaken(string username)
        {
            return await _context.Accounts.AnyAsync(a => a.UserName == username);
        }

        public async Task<User?> RegisterAsync(RegisterDto registerDto)
        {
            string salt = _config.GetValue<string>("PasswordSalt") ?? "";

            var passwordHash = PasswordHasherExtensions.HashPasword(registerDto.Password, salt);

            var newUser = new User
            {
                Email = registerDto.UserName,
                IsInputInformation = false,
            };
            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();
            _context.Accounts.Add(
                new Account
                {
                    UserName = registerDto.UserName,
                    Password = passwordHash,
                    UserId = newUser.Id,
                    IsActive = true
                }
            );
            await _context.SaveChangesAsync();
            return newUser;

        }

        public async Task<bool> ChangePassword(Account account, string newPassword)
        {
            string salt = _config.GetValue<string>("PasswordSalt") ?? "";
            var passwordHash = PasswordHasherExtensions.HashPasword(newPassword, salt);

            account.Password = passwordHash;
            _context.Accounts.Update(account);

            await _context.SaveChangesAsync();

            return true;
        }

        #region Token
        public Token? GenerateToken(string userName, string? role = "User")
        {
            return GenerateJWTTokens(userName, role);
        }

        public Token? GenerateRefreshToken(string username, string? role = "User")
        {
            return GenerateJWTTokens(username, role);
        }

        public Token? GenerateJWTTokens(string userName, string? role = "User")
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var tokenKey = Encoding.UTF8.GetBytes(
                    _config.GetValue<string>("JWT:Key") ?? "sample-key"
                );
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(
                        new Claim[]
                        {
                            new Claim(ClaimTypes.Name, userName),
                            new Claim(ClaimTypes.Role, role ?? "User")
                        }
                    ),
                    Expires = DateTime.Now.AddSeconds(_config.GetValue<int>("JWT:Expire")),
                    SigningCredentials = new SigningCredentials(
                        new SymmetricSecurityKey(tokenKey),
                        SecurityAlgorithms.HmacSha256Signature
                    ),
                    NotBefore = DateTime.Now,
                    Audience = _config.GetValue<string>("JWT:Issuer"),
                    Issuer = _config.GetValue<string>("JWT:Issuer")
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                var refreshToken = GenerateRefreshToken();
                return new Token
                {
                    AccessToken = tokenHandler.WriteToken(token),
                    RefreshToken = refreshToken
                };
            }
            catch (Exception)
            {
                return null;
            }
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var Key = Encoding.UTF8.GetBytes(_config.GetValue<string>("JWT:Key") ?? "sample-key");

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Key),
                ClockSkew = TimeSpan.Zero
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(
                token,
                tokenValidationParameters,
                out SecurityToken securityToken
            );

            JwtSecurityToken? jwtSecurityToken = securityToken as JwtSecurityToken;

            if (
                jwtSecurityToken == null
                || !jwtSecurityToken.Header.Alg.Equals(
                    SecurityAlgorithms.HmacSha256,
                    StringComparison.InvariantCultureIgnoreCase
                )
            )
            {
                throw new SecurityTokenException("Invalid token");
            }

            return principal;
        }

        public int AddUserRefreshTokens(UserRefreshToken user)
        {
            if (user == null || user?.UserName == null)
                throw new ArgumentNullException("");

            _cache.Set(user.UserName, user);

            return 1;
        }

        public UserRefreshToken? GetSavedRefreshTokens(string username, string refreshtoken)
        {
            var token = _cache.Get<UserRefreshToken>(username);

            return token;
        }

        public void RevokeToken(string username, string refreshtoken)
        {
            _cache.Remove(username);
        }

        #endregion
    }
}
