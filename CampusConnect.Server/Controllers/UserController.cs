using CampusConnect.Server.Data;
using CampusConnect.Server.Models;
using CampusConnect.Server.Models.Dtos;
using CampusConnect.Server.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using IAuthService = CampusConnect.Server.Interfaces.IAuthorizationService;

namespace CampusConnect.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly CampusConnectContext _context;
        private readonly IAuthService _authService;
        private readonly ILogger<ControllerBase> _logger;
        private readonly TwoFactorService _twoFactorService;

        public UserController(CampusConnectContext context,
            IAuthService authService,
            ILogger<ControllerBase> logger,
            TwoFactorService twoFactorService)
        {
            _context = context;
            _authService = authService;
            _logger = logger;
            _twoFactorService = twoFactorService;
        }

        [HttpGet("{userId}")]
        public async Task<ActionResult<UserModel>> GetUserById(int userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == userId);
            return user is not null ? Ok(user) : NotFound();
        }

        [HttpGet("exists/{loginName}")]
        public async Task<ActionResult<bool>> LoginNameAlreadyExists(string loginName)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.LoginName == loginName);
            return user is not null;
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserModel>> UserLoginRequest(LoginDto loginDto)
        {
            var user = await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.LoginName == loginDto.LoginName);

            if (user is null)
            {
                return NotFound("User not found.");
            }

            if (_authService.Authorize(user, loginDto))
            {
                if (user.TwoFactorEnabled)
                {
                    var tempToken = _authService.GenerateTempToken(user);
                    return Ok(new LoginResponseDto
                    {
                        RequiresTwoFactor = true,
                        TempToken = tempToken,
                        Username = user.LoginName,
                        Role = user.Role
                    });
                }

                var jwtToken = _authService.GenerateJwtToken(user);
                return Ok(new LoginResponseDto
                {
                    Token = jwtToken,
                    Username = user.LoginName,
                    Role = user.Role
                });
            }

            return BadRequest("Invalid credentials.");
        }


        [HttpDelete("{userId}")]
        public async Task<IActionResult> DeleteUser(int userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == userId);

            if (user is null)
            {
                return BadRequest();
            }

            _context.Remove(user);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            if (_context.Users.Any(u => u.LoginName == model.LoginName))
            {
                return BadRequest("LoginName already exists.");
            }

            var user = new UserModel
            {
                LoginName = model.LoginName,
                Nickname = model.Nickname,
                Role = _context.UserRoles.Find(1) //TODO: F�rs erste Hardcoded auf Admin-Rolle
            };

            var passwordHasher = new PasswordHasher<UserModel>(); //nutzt PBKDF2
            user.PasswordHash = passwordHasher.HashPassword(user, model.Password);

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok("User registered successfully.");
        }

        [HttpPost("2fa/setup")]
        public async Task<ActionResult<TwoFactorSetupDto>> SetupTwoFactor([FromBody] TwoFactorVerifyDto request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.LoginName == request.LoginName);
            
            if (user == null) return NotFound("User not found");

            var secret = _twoFactorService.GenerateSecret();
            var qrCodeUri = _twoFactorService.GenerateQrCodeUri(secret, user.LoginName);
            var qrCodeImage = _twoFactorService.GenerateQrCodeImage(qrCodeUri);

            user.TwoFactorSecret = secret;
            await _context.SaveChangesAsync();

            return Ok(new TwoFactorSetupDto
            {
                QrCodeUri = $"data:image/png;base64,{qrCodeImage}",
                ManualEntryKey = secret
            });
        }

        [HttpPost("2fa/verify")]
        public async Task<IActionResult> VerifyTwoFactor([FromBody] TwoFactorVerifyDto dto)
        {
            if (string.IsNullOrEmpty(dto.LoginName) || string.IsNullOrEmpty(dto.Code))
                return BadRequest("LoginName and Code are required");

            var user = await _context.Users.FirstOrDefaultAsync(u => u.LoginName == dto.LoginName);
            
            if (user?.TwoFactorSecret == null) return BadRequest("User not found or 2FA not set up");

            if (_twoFactorService.ValidateCode(user.TwoFactorSecret, dto.Code))
            {
                user.TwoFactorEnabled = true;
                await _context.SaveChangesAsync();
                return Ok("2FA enabled successfully");
            }

            return BadRequest("Invalid code");
        }

        [HttpPost("2fa/login")]
        public async Task<ActionResult<LoginResponseDto>> VerifyTwoFactorLogin([FromBody] TwoFactorVerifyDto dto)
        {
            if (!_authService.ValidateTempToken(dto.TempToken, out var loginName))
                return BadRequest("Invalid temp token");

            var user = await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.LoginName == loginName);

            if (user?.TwoFactorSecret == null || !user.TwoFactorEnabled)
                return BadRequest();

            if (_twoFactorService.ValidateCode(user.TwoFactorSecret, dto.Code))
            {
                var jwtToken = _authService.GenerateJwtToken(user);
                return Ok(new LoginResponseDto
                {
                    Token = jwtToken,
                    Username = user.LoginName,
                    Role = user.Role
                });
            }

            return BadRequest("Invalid code");
        }

        [HttpPost("2fa/disable")]
        [Authorize]
        public async Task<IActionResult> DisableTwoFactor([FromBody] TwoFactorDisableDto dto)
        {
            var userId = _authService.GetUserIdFromToken(HttpContext);
            var user = await _context.Users.FindAsync(userId);
            
            if (user?.TwoFactorSecret == null) return BadRequest();

            if (_twoFactorService.ValidateCode(user.TwoFactorSecret, dto.Code))
            {
                user.TwoFactorEnabled = false;
                user.TwoFactorSecret = null;
                await _context.SaveChangesAsync();
                return Ok();
            }

            return BadRequest("Invalid code");
        }
    }
}

