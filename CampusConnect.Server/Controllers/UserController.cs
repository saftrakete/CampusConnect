using CampusConnect.Server.Data;
using CampusConnect.Server.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CampusConnect.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly CampusConnectContext _context;
        private readonly ILogger<ControllerBase> _logger;

        public UserController(CampusConnectContext context, ILogger<ControllerBase> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpPost("module")]
        public async Task<ActionResult<Module>> postModules(Module mod) {
            _logger.LogInformation("Ja moin digga\n");
            if (mod is null) {
                return BadRequest();
            }

            _logger.LogInformation("Ja moin digga\n");

            await _context.Modules.AddAsync(mod);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Ja moin digga\n");
            return CreatedAtAction("GetModuleById", new { moduleId = mod.ModuleId }, mod);
        }

        [HttpGet("{moduleId}")]
        public async Task<ActionResult<Module>> GetModuleById(int moduleId) {
            var module = await _context.Modules.FirstOrDefaultAsync(mod => mod.ModuleId == moduleId);
            return module is not null ? Ok(module) : NotFound();
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
            //Passw�rter irgendwie enthashen o.�.
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
