using CampusConnect.Server.Data;
using CampusConnect.Server.Models;
using CampusConnect.Server.Models.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CampusConnect.Server.Controllers
{
    [ApiController]
    //[Route("[controller]")]
    public class ModuleController : ControllerBase
    {
        private readonly CampusConnectContext _context;
        private readonly ILogger<ModuleController> _logger;

        public ModuleController(CampusConnectContext context, ILogger<ModuleController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet("modules/get/all")]
        public async Task<ActionResult<IEnumerable<Module>>> GetAllModules()
        {
            var result = await _context.Modules.ToListAsync();
            _logger.LogInformation($"Got all Modules: result is null? {result is null}");

            return result is not null ? Ok(result) : BadRequest();
        }

        [HttpGet("modules/get/{moduleId}")]
        public async Task<ActionResult<Module>> GetModuleById(int moduleId)
        {
            var module = await _context.Modules.FirstOrDefaultAsync(module => module.ModuleId == moduleId);
            _logger.LogInformation($"Got Module by Id: result is null? {module is null}");

            return module is not null ? Ok(module) : BadRequest();
        }

        [HttpPost("modules/postModule")]
        public async Task<ActionResult<Module>> PostModule(ModuleDto moduleDto)
        {
            if (moduleDto is null)
            {
                _logger.LogError("Posting module failed: ModuleDto was null");
                return BadRequest();
            }

            var module = ConvertModuleDto(moduleDto);

            _context.Modules.Add(module);
            _logger.LogInformation("Trying to save changes...");

            try
            {
                await _context.SaveChangesAsync();
                _logger.LogInformation("Successfully saved changes");
                return CreatedAtAction("GetModuleById", new { moduleId = module.ModuleId });
            }
            catch
            {
                _logger.LogError("Saving changes failed");
                return BadRequest();
            }
        }

        [HttpPut("modules/edit/{moduleId}")]
        public async Task<ActionResult<Module>> UpdateModule(int moduleId, Module module)
        {
            if (module.ModuleId != moduleId)
            {
                _logger.LogError("Updating module failed: Ids did not match");
                return BadRequest();
            }

            _context.Modules.Entry(module).State = EntityState.Modified;
            _logger.LogInformation("Trying to save changes...");

            try
            {
                await _context.SaveChangesAsync();
                _logger.LogInformation("Successfully saved changes");
                return Ok(module);
            }
            catch (DbUpdateConcurrencyException)
            {
                _logger.LogError("Saving changes failed");

                if (!ModuleExists(moduleId))
                {
                    return BadRequest();
                }
                else
                {
                    throw;
                }
            }
        }

        [HttpDelete("modules/delete/{moduleId}")]
        public async Task<ActionResult> DeleteModule(int moduleId)
        {
            var module = await _context.Modules.FindAsync(moduleId);

            if (module is null)
            {
                return BadRequest();
            }

            _context.Modules.Remove(module);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ModuleExists(int moduleId) 
        {
            return _context.Modules.Any(m => m.ModuleId == moduleId);
        }

        private Module ConvertModuleDto(ModuleDto moduleDto)
        {
            var faculty = _context.Faculties.Find(moduleDto.FacultyId);

            return new Module(moduleDto.Name, faculty, moduleDto.Difficulty);
        }
    }
}