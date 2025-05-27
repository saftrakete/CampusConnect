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
    public class FacultyController : ControllerBase
    {
        private readonly CampusConnectContext _context;

        public FacultyController(CampusConnectContext context)
        {
            _context = context;
        }

        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<Faculty>>> GetFaculties()
        {
            var result = await _context.Faculties.ToListAsync();

            return result is not null ? Ok(result) : NotFound();
        }

        [HttpGet("{facultyId}")]
        public async Task<ActionResult<Faculty>> GetFacultyById(int facultyId)
        {
            var faculty = await _context.Faculties
                .Where(faculty => faculty.FacultyId == facultyId)
                .FirstOrDefaultAsync();

            return faculty is not null ? Ok(faculty) : NotFound();
        }

        [HttpPost("addFaculty")]
        public async Task<ActionResult<Faculty>> PostNewFaculty(Faculty faculty)
        {
            if (faculty is null)
            {
                return BadRequest();
            }

            _context.Faculties.Add(faculty);

            try
            {
                await _context.SaveChangesAsync();
                return CreatedAtAction("GetFacultyById", new { facultyId = faculty.FacultyId });
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPut("update/{facultyId}")]
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

        [HttpDelete("delete/{facultyId}")]
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
    }
}
