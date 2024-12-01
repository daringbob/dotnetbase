using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using src.Dtos.Auth;
using src.Extensions;
using src.Models;
using src.Repositories.Auth;

namespace src.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _authRepo;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;

        public AuthController(IAuthRepository authRepo, IMapper mapper, IConfiguration config)
        {
            _authRepo = authRepo;
            _mapper = mapper;
            _config = config;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginData)
        {
            var account = await _authRepo.GetAccountByUserName(loginData.UserName);
            if (account == null)
            {
                return BadRequest("Incorrect username or password!");
            }

            var isPasswordValid = _authRepo.ValidatePassword(loginData.Password, account.Password);
            if (!isPasswordValid)
            {
                return BadRequest("Incorrect username or password!");
            }

            if (account.IsActive == false)
            {
                return Unauthorized("Account is not active");
            }

            User? user = null;
            List<Permission?>? permission = null;
            if (account.UserId != 0)
            {
                user = await _authRepo.GetUserById(account.UserId);

                if (user == null || user.IsActive == false)
                {
                    return Unauthorized("User is not active");
                }

                permission = await _authRepo.GetPermissionsOfUser(account.UserId);
            }

            // Generate and return JWT token
            var token = _authRepo.GenerateToken(loginData.UserName, user?.Roles?.Title);
            if (token == null)
            {
                return BadRequest("Invalid Attempt!");
            }

            // saving refresh token to the db
            var obj = new UserRefreshToken
            {
                RefreshToken = token.RefreshToken,
                UserName = loginData.UserName
            };
            _authRepo.AddUserRefreshTokens(obj);

            var response = new LoginResDto
            {
                Token = token,
                Account = _mapper.Map<AccountDto>(account),
                Permission = permission,
                User = user
            };

            return Ok(response);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerData)
        {
            var isUsernameTaken = await _authRepo.IsUsernameTaken(registerData.UserName);
            if (isUsernameTaken)
            {
                return BadRequest("Username already taken!");
            }

            try
            {
                var newUser = await _authRepo.RegisterAsync(registerData);
                var token = _authRepo.GenerateToken(registerData.UserName, newUser?.Roles?.Title);
                if (token == null)
                {
                    return BadRequest("Invalid Attempt!");
                }
                var obj = new UserRefreshToken
                {
                    RefreshToken = token.RefreshToken,
                    UserName = registerData.UserName
                };
                _authRepo.AddUserRefreshTokens(obj);
                var account = await _authRepo.GetAccountByUserName(registerData.UserName);
                var permission = await _authRepo.GetPermissionsOfUser(account.UserId);
                var response = new LoginResDto
                {
                    Token = token,
                    Account = _mapper.Map<AccountDto>(account),
                    Permission = permission,
                    User = newUser
                };

                return Ok(response);
            }
            catch (System.Exception)
            {

                return BadRequest("Failed to register account.");
            }


        }

        [Authorize]
        [HttpPost("me")]
        public async Task<IActionResult> Me([FromBody] Token userToken)
        {
            if (User == null)
            {
                return NotFound();
            }

            var userName = User.Identity?.Name;
            var tokenRole = User.FindFirstValue(ClaimTypes.Role);

            if (String.IsNullOrEmpty(userName))
            {
                return NotFound();
            }

            var account = await _authRepo.GetAccountByUserName(userName);

            if (account == null)
            {
                return NotFound();
            }

            User? user = null;
            List<Permission?>? permission = null;
            if (account.AccountType == AccountType.User && account.UserId != 0)
            {
                user = await _authRepo.GetUserById(account.UserId);
                permission = await _authRepo.GetPermissionsOfUser(account.UserId);
            }

            Token? token = null;
            // Role changed, client needs to use the new token
            if (user?.Roles?.Title != tokenRole)
            {
                token = _authRepo.GenerateToken(userName, user?.Roles?.Title);
                if (token == null)
                {
                    return BadRequest("Invalid Attempt!");
                }

                // saving refresh token to the db
                var obj = new UserRefreshToken
                {
                    RefreshToken = token.RefreshToken,
                    UserName = userName
                };

                _authRepo.RevokeToken(userName, userToken.RefreshToken);
                _authRepo.AddUserRefreshTokens(obj);
            }

            var response = new LoginResDto
            {
                Token = token,
                Account = _mapper.Map<AccountDto>(account),
                Permission = permission,
                User = user
            };

            return Ok(response);
        }

        [HttpPost("refresh")]
        public IActionResult RefreshToken([FromBody] Token token)
        {
            var principal = _authRepo.GetPrincipalFromExpiredToken(token.AccessToken);
            var username = principal.Identity?.Name;
            var tokenRole = principal.FindFirstValue(ClaimTypes.Role);

            if (username == null)
            {
                return BadRequest("Invalid attempt!");
            }

            //retrieve the saved refresh token from database
            var savedRefreshToken = _authRepo.GetSavedRefreshTokens(username, token.RefreshToken);

            if (savedRefreshToken == null || savedRefreshToken.RefreshToken != token.RefreshToken)
            {
                return BadRequest("Invalid attempt!");
            }

            var newJwtToken = _authRepo.GenerateRefreshToken(username, tokenRole);

            if (newJwtToken == null)
            {
                return BadRequest("Invalid attempt!");
            }

            // saving refresh token to the db
            UserRefreshToken obj = new UserRefreshToken
            {
                RefreshToken = newJwtToken.RefreshToken,
                UserName = username
            };

            _authRepo.RevokeToken(username, token.RefreshToken);
            _authRepo.AddUserRefreshTokens(obj);

            return Ok(newJwtToken);
        }

        [Authorize]
        [HttpPost("hash")]
        public IActionResult HashPassword(string rawstring)
        {
            string salt = _config.GetValue<string>("PasswordSalt") ?? "";
            if (rawstring is not null)
            {
                var passwordHash = PasswordHasherExtensions.HashPasword(rawstring, salt);

                return Ok(new { RawPassword = rawstring, PasswordHash = passwordHash });
            }

            return BadRequest("password is invalid.");
        }

        [Authorize]
        [HttpPut("change-password")]
        //localhost:5001/api/users/{id}
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto request)
        {
            string? userName = User.Identity?.Name;
            if (string.IsNullOrEmpty(userName))
            {
                return NotFound();
            }

            var account = await _authRepo.GetAccountByUserName(userName);
            if (account == null)
            {
                return NotFound();
            }

            //check old password
            var isPasswordValid = _authRepo.ValidatePassword(request.OldPassword, account.Password);
            if (!isPasswordValid)
            {
                return BadRequest("Old password is not correct!");
            }

            var isSuccess = await _authRepo.ChangePassword(account, request.NewPassword);
            if (!isSuccess)
            {
                return BadRequest("Change password fail!");
            }

            return NoContent();
        }
    }
}
