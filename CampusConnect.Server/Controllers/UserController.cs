using CampusConnect.Server.Data;
using CampusConnect.Server.Models;
using CampusConnect.Server.Models.Dtos;
using CampusConnect.Server.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
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

        [HttpGet("getId/{loginName}")]
        public async Task<ActionResult<int>> GetUserIdByLoginName(string loginName)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.LoginName == loginName);
            return user.UserId;
        }

        [HttpGet("check-onboarding-status/{loginName}")]
        public async Task<ActionResult<bool>> CheckOnboardingStatus(string loginName)
        {
            var user = await _context.Users.Include(u => u.UserModules).FirstOrDefaultAsync(u => u.LoginName == loginName);
            return user.UserModules.Count != 0;
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserModel>> UserLoginRequest(LoginDto loginDto)
        {
            _logger.LogInformation("Login attempt for user: {LoginName}", loginDto?.LoginName ?? "null");
            
            if (loginDto == null || string.IsNullOrEmpty(loginDto.LoginName) || string.IsNullOrEmpty(loginDto.Password))
            {
                _logger.LogWarning("Login failed: Missing credentials");
                return BadRequest("LoginName and Password are required");
            }

            try
            {
                var user = await _context.Users
                    .Include(u => u.Role)
                    .FirstOrDefaultAsync(u => u.LoginName == loginDto.LoginName);

                if (user is null)
                {
                    _logger.LogWarning("Login failed: User not found for {LoginName}", loginDto.LoginName);
                    return NotFound("User not found.");
                }

                if (_authService.Authorize(user, loginDto))
                {
                    if (user.TwoFactorEnabled)
                    {
                        var tempToken = _authService.GenerateTempToken(user);
                        _logger.LogInformation("2FA required for user: {LoginName}", loginDto.LoginName);
                        return Ok(new LoginResponseDto
                        {
                            RequiresTwoFactor = true,
                            TempToken = tempToken,
                            Username = user.LoginName,
                            Role = user.Role
                        });
                    }

                    var jwtToken = _authService.GenerateJwtToken(user);
                    _logger.LogInformation("Login successful for user: {LoginName}", loginDto.LoginName);
                    return Ok(new LoginResponseDto
                    {
                        Token = jwtToken,
                        Username = user.LoginName,
                        Role = user.Role
                    });
                }

                _logger.LogWarning("Login failed: Invalid credentials for {LoginName}", loginDto.LoginName);
                return BadRequest("Invalid credentials.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Login error for user: {LoginName}", loginDto.LoginName);
                return StatusCode(500, "Internal server error");
            }
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

        [HttpPost("saveModules/{id}")]
        public async Task<IActionResult> SaveUserModules(Module[] modules, int id)
        {
            var user = await _context.Users.Include(u => u.UserModules).FirstOrDefaultAsync(u => u.UserId == id);

            if (user == null)
            {
                return BadRequest(new { message = "User not found"});
            }

            foreach (var mod in modules)
            {
                var currentModule = await _context.Modules.FindAsync(mod.ModuleId);

                if (currentModule == null)
                {
                    return BadRequest(new { message = "Module not found" });
                }
                else
                {
                    user.UserModules.Add(currentModule);
                }
            }
            await _context.SaveChangesAsync();
            return Ok(user.UserModules.Select(m => m.Name));
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
            _logger.LogInformation("2FA verification attempt for user: {LoginName}", dto.LoginName);
            
            if (string.IsNullOrEmpty(dto.LoginName) || string.IsNullOrEmpty(dto.Code))
                return BadRequest("LoginName and Code are required");

            var user = await _context.Users.FirstOrDefaultAsync(u => u.LoginName == dto.LoginName);
            
            if (user?.TwoFactorSecret == null) 
            {
                _logger.LogWarning("2FA verification failed: User not found or 2FA not set up for {LoginName}", dto.LoginName);
                return BadRequest("User not found or 2FA not set up");
            }

            _logger.LogInformation("Validating TOTP code for user: {LoginName}, Code: {Code}", dto.LoginName, dto.Code);

            if (!_twoFactorService.ValidateCode(user.TwoFactorSecret, dto.Code))
            {
                _logger.LogWarning("2FA verification failed: Invalid code for user: {LoginName}", dto.LoginName);
                return BadRequest("Invalid code");
            }
        
                    user.TwoFactorEnabled = true;
                    await _context.SaveChangesAsync();
                    _logger.LogInformation("2FA enabled successfully for user: {LoginName}", dto.LoginName);
                    return Ok("2FA enabled successfully");
                

            // _logger.LogWarning("2FA verification failed: Invalid code for user: {LoginName}", dto.LoginName);
            // return BadRequest("Invalid code");
        }

        [HttpPost("2fa/login")]
        public async Task<ActionResult<LoginResponseDto>> VerifyTwoFactorLogin([FromBody] TwoFactorVerifyDto dto)
        {
            _logger.LogInformation("2FA login attempt with temp token");
            
            if (!_authService.ValidateTempToken(dto.TempToken, out var loginName))
            {
                _logger.LogWarning("2FA login failed: Invalid temp token");
                return BadRequest("Invalid temp token");
            }

            var user = await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.LoginName == loginName);

            if (user?.TwoFactorSecret == null || !user.TwoFactorEnabled)
            {
                _logger.LogWarning("2FA login failed: User not found or 2FA not enabled for {LoginName}", loginName);
                return BadRequest();
            }

            _logger.LogInformation("Validating 2FA login code for user: {LoginName}", loginName);
            
            if (!_twoFactorService.ValidateCode(user.TwoFactorSecret, dto.Code))
            {
                  _logger.LogWarning("2FA login failed: Invalid code for user: {LoginName}", loginName);
            return BadRequest("Invalid code");
            }

            var jwtToken = _authService.GenerateJwtToken(user);
                _logger.LogInformation("2FA login successful for user: {LoginName}", loginName);
                return Ok(new LoginResponseDto
                {
                    Token = jwtToken,
                    Username = user.LoginName,
                    Role = user.Role
                });
          
        }

        [HttpPost("2fa/disable")]
        [Authorize]
        public async Task<IActionResult> DisableTwoFactor([FromBody] TwoFactorDisableDto dto)
        {
            var userId = _authService.GetUserIdFromToken(HttpContext);
            var user = await _context.Users.FindAsync(userId);
            
            if (user?.TwoFactorSecret == null) return BadRequest();

            if (!_twoFactorService.ValidateCode(user.TwoFactorSecret, dto.Code))
            {
                 return BadRequest("Invalid code");
            }
            
            user.TwoFactorEnabled = false;
                user.TwoFactorSecret = null;
                await _context.SaveChangesAsync();
                return Ok();
           
        }

        [HttpGet("2fa/status")]
        [Authorize]
        public async Task<ActionResult<bool>> GetTwoFactorStatus()
        {
            var userId = _authService.GetUserIdFromToken(HttpContext);
            var user = await _context.Users.FindAsync(userId);
            
            return user?.TwoFactorEnabled ?? false;
        }
    }
}

