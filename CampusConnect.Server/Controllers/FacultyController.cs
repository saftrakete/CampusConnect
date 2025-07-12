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
    [Route("api/faculties")]
    public class FacultyController : ControllerBase
    {
        private readonly CampusConnectContext _context;
        private readonly ILogger<FacultyController> _logger;

        public FacultyController(CampusConnectContext context, ILogger<FacultyController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet("get/all")]
        public async Task<ActionResult<IEnumerable<Faculty>>> GetFaculties()
        {
            var result = await _context.Faculties
                .Include(faculty => faculty.Degrees)
                .Include(faculty => faculty.Modules)
                .ToListAsync();

            _logger.LogInformation($"Got Faculties: result is null? {result is null}");

            return result is not null ? Ok(result) : NotFound();
        }

        [HttpGet("get/{facultyId}", Name = "GetFacultyById")]
        public async Task<ActionResult<Faculty>> GetFacultyById([FromRoute] int facultyId)
        {
            var faculty = await _context.Faculties
                .Include(faculty => faculty.Degrees)
                .Include(faculty => faculty.Modules)
                .Where(faculty => faculty.FacultyId == facultyId)
                .FirstOrDefaultAsync();

            _logger.LogInformation($"Got faculty by Id: result is null? {faculty is null}");

            return faculty is not null ? Ok(faculty) : NotFound();
        }

        [HttpPost("postFaculty")]
        public async Task<ActionResult<Faculty>> PostFaculty([FromBody] FacultyDto facultyDto)
        {
            if (facultyDto is null)
            {
                _logger.LogError("Posting Faculty failed: facultyDto was null");
                return BadRequest();
            }

            var faculty = ConvertFacultyDto(facultyDto);
            _context.Faculties.Add(faculty);
            _logger.LogInformation("Trying to save new faculty...");

            try
            {
                await _context.SaveChangesAsync();
                _logger.LogInformation("Successfully saved changes");
                return Created($"api/faculties/get/{faculty.FacultyId}", faculty);
            }
            catch
            {
                _logger.LogError("Saving changes failed");
                return BadRequest();
            }
        }

        [HttpPut("edit/{facultyId}")]
        public async Task<ActionResult<Faculty>> UpdateFaculty([FromRoute] int facultyId, [FromBody] FacultyDto facultyDto)
        {
            var faculty = ConvertFacultyDto(facultyDto);

            if (faculty is null)
            {
                _logger.LogError("Updating Faculty failed: facultyDto was null");
                return BadRequest();
            }

            if (facultyId != faculty.FacultyId)
            {
                _logger.LogError("Updating Faculty failed: Ids did not match");
                return BadRequest();
            }

            _context.Faculties.Entry(faculty).State = EntityState.Modified;

            try
            {
                _logger.LogInformation("Trying to save changes...");
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FacultyExists(facultyId))
                {
                    _logger.LogError("Updating Faculty failed: Faculty does not exist");
                    return BadRequest();
                }
                else
                {
                    _logger.LogError("Updating Faculty failed: Concurrency exception occurred");
                    throw;
                }
            }

            _logger.LogInformation("Successfully saved changes");
            return Ok(faculty);
        }

        [HttpDelete("delete/{facultyId}")]
        public async Task<IActionResult> DeleteFaculty([FromRoute] int facultyId)
        {
            var faculty = await _context.Faculties.FirstOrDefaultAsync(faculty => faculty.FacultyId == facultyId);

            if (faculty is null)
            {
                _logger.LogError("Deleting Faculty failed: faculty was null");
                return BadRequest();
            }

            _context.Faculties.Remove(faculty);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Successfully deleted Faculty");
            return NoContent();
        }

        private bool FacultyExists(int facultyId)
        {
            return _context.Faculties.Any(faculty => faculty.FacultyId == facultyId);
        }

        private Faculty ConvertFacultyDto(FacultyDto facultyDto)
        {
            var degrees = _context.Degrees
                .Where(degree => facultyDto.DegreeIds.Contains(degree.DegreeId))
                .ToList();

            var modules = _context.Modules
                .Where(module => facultyDto.ModuleIds.Contains(module.ModuleId))
                .ToList();

            return new Faculty(facultyDto.Name, facultyDto.FacultyCode, degrees, modules);
        }
    }
}
