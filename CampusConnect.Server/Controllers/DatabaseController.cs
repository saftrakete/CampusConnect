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
        private readonly ILogger<DatabaseController> _logger;

        public DatabaseController(CampusConnectContext context, ILogger<DatabaseController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // Füllt Modul-Tabelle mit einigen Testmodulen auf
        [HttpPost("module")]
        public async Task<ActionResult<Module>> postModules(Module mod) {
            if (mod is null) {
                return BadRequest();
            }

            await _context.Modules.AddAsync(mod);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetModuleById", new { moduleId = mod.ModuleId }, mod);
        }

        [HttpGet("{moduleId}")]
        public async Task<ActionResult<Module>> GetModuleById(int moduleId) {
            var module = await _context.Modules.FirstOrDefaultAsync(mod => mod.ModuleId == moduleId);
            return module is not null ? Ok(module) : NotFound();
        }

        [HttpDelete("cleanTable")]
        public Task<IActionResult> ResetModuleDatabase() {

            // Löscht alle Daten aus der Datenbank und setzt ID-Indexe zurück
            _context.Database.ExecuteSqlRaw("TRUNCATE TABLE [Modules]");

            return NoContent();
        }

    }
}