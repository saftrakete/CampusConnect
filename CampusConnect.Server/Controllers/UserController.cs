using CampusConnect.Server.Data;
using CampusConnect.Server.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        public UserController(CampusConnectContext context)
        {
            _context = context;
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
            var user = await _context.Users.FirstOrDefaultAsync(u => u.LoginName == loginDto.LoginName);

            if (user is null)
            {
                return NotFound();
            }

            //TODO:
            //Passwörter irgendwie enthashen o.ä.
            //User authorisieren

            return user.Password == loginDto.Password ? Ok(user) : BadRequest();
        }

        [HttpPost]
        public async Task<ActionResult<IEnumerable<UserModel>>> PostNewUser(UserModel user)
        {
            if (user is null)
            {
                return BadRequest();
            }

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUserById", new { userId = user.UserId }, user);
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
    }
}
