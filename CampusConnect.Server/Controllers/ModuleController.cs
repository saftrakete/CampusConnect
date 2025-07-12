using CampusConnect.Server.Data;
using CampusConnect.Server.Enums;
using CampusConnect.Server.Models;
using CampusConnect.Server.Models.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CampusConnect.Server.Controllers
{
    [ApiController]
    [Route("api/modules")]
    public class ModuleController : ControllerBase
    {
        private readonly CampusConnectContext _context;
        private readonly ILogger<ModuleController> _logger;

        public ModuleController(CampusConnectContext context, ILogger<ModuleController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet("get/all")]
        public async Task<ActionResult<IEnumerable<Module>>> GetAllModules()
        {
            var result = await _context.Modules.ToListAsync();

            _logger.LogInformation($"Got all Modules: result is null? {result is null}");

            return result is not null ? Ok(result) : BadRequest();
        }

        [HttpGet("get/{moduleId}")]
        public async Task<ActionResult<Module>> GetModuleById([FromRoute] int moduleId)
        {
            var module = await _context.Modules.FirstOrDefaultAsync(module => module.ModuleId == moduleId);

            _logger.LogInformation($"Got Module by Id: result is null? {module is null}");

            return module is not null ? Ok(module) : BadRequest();
        }

        [HttpPost("postModule")]
        public async Task<ActionResult<Module>> PostModule([FromBody] ModuleDto moduleDto)
        {
            if (moduleDto is null)
            {
                _logger.LogError("Posting module failed: ModuleDto was null");
                return BadRequest();
            }

            var module = ConvertModuleDto(moduleDto);

            var faculty = await _context.Faculties.FindAsync(moduleDto.FacultyId);

            if (faculty is null)
            {
                _logger.LogError("Posting module failed: Invalid facultyId");
                return BadRequest();
            }

            faculty.Modules.Add(module);
            _context.Faculties.Entry(faculty).State = EntityState.Modified;

            _context.Modules.Add(module);
            _logger.LogInformation("Trying to save changes...");

            try
            {
                await _context.SaveChangesAsync();
                _logger.LogInformation("Successfully saved changes");
                return Created($"api/modules/get/{module.ModuleId}", module);
            }
            catch
            {
                _logger.LogError("Saving changes failed");
                return BadRequest();
            }
        }

        [HttpPut("edit/{moduleId}")]
        public async Task<ActionResult<Module>> UpdateModule([FromRoute] int moduleId, [FromBody] ModuleDto moduleDto)
        {
            var module = ConvertModuleDto(moduleDto);

            if (module is null)
            {
                _logger.LogError("Updating module failed: ModuleDto was null");
                return BadRequest();
            }

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
                    _logger.LogError($"Module with Id {moduleId} does not exist");
                    return BadRequest();
                }
                else
                {
                    _logger.LogError("Concurrency exception occurred while updating module");
                    throw;
                }
            }
        }

        [HttpDelete("delete/{moduleId}")]
        public async Task<ActionResult> DeleteModule([FromRoute] int moduleId)
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
            var difficulty = ConvertStringToDifficultyEnum(moduleDto.Difficulty);

            return new Module(moduleDto.Name, moduleDto.Description, difficulty);
        }

        private DifficultyEnum ConvertStringToDifficultyEnum(string difficultyString)
        {
            switch (difficultyString.ToLower())
            {
                case "easy":
                    return DifficultyEnum.Easy;
                case "medium":
                    return DifficultyEnum.Medium;
                case "hard":
                    return DifficultyEnum.Hard;
                default:
                    _logger.LogError($"Invalid difficulty string: {difficultyString}");
                    throw new ArgumentException("Invalid difficulty string");
            }
        }
    }
}