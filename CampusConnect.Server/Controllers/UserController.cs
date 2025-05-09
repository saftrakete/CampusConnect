using CampusConnect.Server.Data;
using CampusConnect.Server.Interfaces;
using CampusConnect.Server.Models;
using CampusConnect.Server.Models.Dtos;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CampusConnect.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly CampusConnectContext _context;
        private readonly IAuthorizationService _authService;

        public UserController(CampusConnectContext context,
            IAuthorizationService authService)
        {
            _context = context;
            _authService = authService;
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

            //TODO: in AuthService verschieben
            var passwordHasher = new PasswordHasher<UserModel>();
            var verificationResult = passwordHasher.VerifyHashedPassword(user, user.PasswordHash, loginDto.Password);
            //var userRole = _context.UserRoles.Find()

            if (verificationResult == PasswordVerificationResult.Success)
            {
                //return Ok(user);
                var jwtToken = _authService.AuthorizeAndGetToken(user);

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
                Role = _context.UserRoles.Find(1)
            };

            var passwordHasher = new PasswordHasher<UserModel>(); //nutzt PBKDF2
            user.PasswordHash = passwordHasher.HashPassword(user, model.Password);

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok("User registered successfully.");
        }
    }
}

