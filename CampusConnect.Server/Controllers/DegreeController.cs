using CampusConnect.Server.Data;
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
    [Route("api/degrees")]
    public class DegreeController : ControllerBase
    {
        private readonly CampusConnectContext _context;
        private readonly ILogger<ModuleController> _logger;

        public DegreeController(CampusConnectContext context, ILogger<ModuleController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet("get/all")]
        public async Task<ActionResult<IEnumerable<Degree>>> GetDegrees()
        {
            var result = await _context.Degrees
                .Include(degree => degree.MandatoryModules)
                .ToListAsync();

            _logger.LogInformation($"Got all Degrees: result is null? {result is null}");
            return result is not null ? Ok(result) : NotFound();
        }

        [HttpGet("get/{degreeId}", Name = "GetDegreeById")]
        public async Task<ActionResult<Degree>> GetDegreeById([FromRoute] int degreeId)
        {
            var degree = await _context.Degrees
                .Where(degree => degree.DegreeId == degreeId)
                .Include(degree => degree.MandatoryModules)
                .FirstOrDefaultAsync();

            _logger.LogInformation($"Got Degree by Id: result is null? {degree is null}");
            return degree is not null ? degree : NotFound();
        }

        [HttpPost("postDegree")]
        public async Task<ActionResult<Degree>> PostNewDegree([FromBody] DegreeDto degreeDto)
        {
            if (degreeDto is null)
            {
                _logger.LogError("Posting degree failed: DegreeDto was null");
                return BadRequest();
            }

            var degree = ConvertDegreeDto(degreeDto);

            var faculty = await _context.Faculties
                .Where(f => f.FacultyId == degreeDto.FacultyId)
                .FirstOrDefaultAsync();

            if (faculty is null)
            {
                _logger.LogError("Posting degree failed: Invalid facultyId");
                return BadRequest();
            }

            faculty.Degrees.Add(degree);
            _context.Faculties.Entry(faculty).State = EntityState.Modified;

            if (degree is null)
            {
                _logger.LogError("Posting degree failed: Degree conversion failed");
                return BadRequest();
            }

            if (!ValidateDegreeObject(degree))
            {
                _logger.LogError("Posting degree failed: Degree validation failed");
                return BadRequest();
            }

            _context.Degrees.Add(degree);

            try
            {
                _logger.LogInformation("Trying to save changes...");
                await _context.SaveChangesAsync();
                _logger.LogInformation("Successfully saved changes");
                return Created($"/api/degrees/get/{degree.DegreeId}", degree);
            }
            catch
            {
                _logger.LogError("Saving changes failed");
                return BadRequest();
            }
        }

        [HttpPut("edit/{degreeId}")]
        public async Task<ActionResult<Degree>> UpdateDegree([FromRoute] int degreeId, [FromBody] DegreeDto degreeDto)
        {
            var degree = ConvertDegreeDto(degreeDto);

            if (degree is null)
            {
                _logger.LogError("Updating degree failed: DegreeDto was null");
                return BadRequest();
            }

            if (degreeId != degree.DegreeId)
            {
                _logger.LogError("Updating degree failed: Ids did not match");
                return BadRequest();
            }

            _context.Entry(degree).State = EntityState.Modified;

            try
            {
                _logger.LogInformation("Trying to save changes...");
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DegreeExists(degreeId))
                {
                    _logger.LogError("Updating degree failed: Degree does not exist");
                    return BadRequest();
                }
                else
                {
                    _logger.LogError("Updating degree failed: Concurrency exception");
                    throw;
                }
            }

            _logger.LogInformation("Successfully saved changes");
            return Ok(degree);
        }

        [HttpDelete("delete/{degreeId}")]
        public async Task<ActionResult> DeleteDegree([FromRoute] int degreeId)
        {
            var degree = await _context.Degrees.FirstOrDefaultAsync(degree => degree.DegreeId == degreeId);

            if (degree is null)
            {
                _logger.LogError("Deleting Degree failed: Degree was null");
                return BadRequest();
            }

            _context.Degrees.Remove(degree);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Successfully deleted Degree");
            return NoContent();
        }

        private bool DegreeExists(int degreeId)
        {
            return _context.Degrees.Any(degree => degree.DegreeId == degreeId);
        }

        private Degree ConvertDegreeDto(DegreeDto degreeDto)
        {
            var mandatoryModules = _context.Modules
                .Where(module => degreeDto.MandatoryModuleIds.Contains(module.ModuleId))
                .ToList();

            return new Degree(degreeDto.Name, mandatoryModules);
        }

        private Boolean ValidateDegreeObject(Degree degree)
        {
            if (degree.MandatoryModules is null)
            {
                _logger.LogError("Degree validation failed: MandatoryModules is null");
                return false;
            }

            return true;
        }
    }
}
