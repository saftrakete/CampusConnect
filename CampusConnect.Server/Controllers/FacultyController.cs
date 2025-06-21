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
    public class FacultyController : ControllerBase
    {
        private readonly CampusConnectContext _context;
        private readonly ILogger<ModuleController> _logger;

        public FacultyController(CampusConnectContext context, ILogger<ModuleController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet("faculties/get/all")]
        public async Task<ActionResult<IEnumerable<Faculty>>> GetFaculties()
        {
            var result = await _context.Faculties.ToListAsync();
            _logger.LogInformation($"Got Faculties: result is null? {result is null}");

            return result is not null ? Ok(result) : NotFound();
        }

        [HttpGet("faculties/get/{facultyId}")]
        public async Task<ActionResult<Faculty>> GetFacultyById(int facultyId)
        {
            var faculty = await _context.Faculties
                .Where(faculty => faculty.FacultyId == facultyId)
                .FirstOrDefaultAsync();

            _logger.LogInformation($"Got faculty by Id: result is null? {faculty is null}");

            return faculty is not null ? Ok(faculty) : NotFound();
        }

        [HttpPost("faculties/postFaculty")]
        //[Route("faculties/postFaculty")]
        public async Task<ActionResult<Faculty>> PostNewFaculty(FacultyDto facultyDto)
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
                return CreatedAtAction("GetFacultyById", new { facultyId = faculty.FacultyId });
            }
            catch
            {
                _logger.LogError("Saving changes failed");
                return BadRequest();
            }
        }

        [HttpPut("faculties/edit/{facultyId}")]
        public async Task<ActionResult<Faculty>> UpdateFaculty(int facultyId, Faculty faculty)
        {
            if (facultyId != faculty.FacultyId)
            {
                return BadRequest();
            }

            _context.Faculties.Entry(faculty).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FacultyExists(facultyId))
                {
                    return BadRequest();
                }
                else
                {
                    throw;
                }
            }

            return Ok(faculty);
        }

        [HttpDelete("faculties/delete/{facultyId}")]
        public async Task<IActionResult> DeleteFaculty(int facultyId)
        {
            var faculty = await _context.Faculties.FirstOrDefaultAsync(faculty => faculty.FacultyId == facultyId);

            if (faculty is null)
            {
                return BadRequest();
            }

            _context.Faculties.Remove(faculty);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool FacultyExists(int facultyId)
        {
            return _context.Faculties.Any(faculty => faculty.FacultyId == facultyId);
        }

        private Faculty ConvertFacultyDto(FacultyDto facultyDto)
        {
            return new Faculty(facultyDto.Name);
        }
    }
}
