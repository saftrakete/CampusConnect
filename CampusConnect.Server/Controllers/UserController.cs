using CampusConnect.Server.Data;
using CampusConnect.Server.Interfaces;
using CampusConnect.Server.Models;
using CampusConnect.Server.Models.Dtos;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

namespace CampusConnect.Server.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly CampusConnectContext _context;
        private readonly IAuthorizationService _authService;
        private readonly ILogger<ControllerBase> _logger;

        public UserController(CampusConnectContext context,
            IAuthorizationService authService,
            ILogger<ControllerBase> logger)
        {
            _context = context;
            _authService = authService;
            _logger = logger;
        }

        [HttpGet("get/{userId}")]
        public async Task<ActionResult<UserModel>> GetUserById(int userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == userId);

            _logger.LogInformation($"Got User by Id: result is null? {user is null}");

            return user is not null ? Ok(user) : NotFound();
        }

        [HttpGet("exists/{loginName}")]
        public async Task<ActionResult<bool>> LoginNameAlreadyExists(string loginName)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.LoginName == loginName);

            _logger.LogInformation($"Checking if login name exists: {loginName} - result is null? {user is null}");

            return Ok(user is not null);
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


        [HttpDelete("delete/{userId}")]
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
    }
}

