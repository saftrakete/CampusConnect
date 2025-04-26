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
    public class DatabaseController : ControllerBase
    {
        private readonly CampusConnectContext _context;
        private readonly ILogger<ControllerBase> _logger;

        public DatabaseController(CampusConnectContext context, ILogger<ControllerBase> logger)
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
    }
}