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
    //[Route("[controller]")]
    [ApiController]
    //TODO: Logging
    public class DegreeController : ControllerBase
    {
        private readonly CampusConnectContext _context;
        private readonly ILogger<ModuleController> _logger;

        public DegreeController(CampusConnectContext context, ILogger<ModuleController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet("degrees/get/all")]
        public async Task<ActionResult<IEnumerable<Degree>>> GetDegrees()
        {
            var result = await _context.Degrees
                .Include(degree => degree.Faculty)
                .Include(degree => degree.MandatoryModules)
                .ToListAsync();

            return result is not null ? Ok(result) : NotFound();
        }

        [HttpGet("degrees/get/{degreeId}")]
        public async Task<ActionResult<Degree>> GetDegreeById(int degreeId)
        {
            var degree = await _context.Degrees
                .Where(degree => degree.DegreeId == degreeId)
                .Include(degree => degree.Faculty)
                .Include(degree => degree.MandatoryModules)
                .FirstOrDefaultAsync();

            return degree is not null ? degree : NotFound();
        }

        [HttpPost("degrees/postDegree")]
        public async Task<ActionResult<Degree>> PostNewDegree(DegreeDto degreeDto)
        {
            if (degreeDto is null)
            {
                return BadRequest();
            }

            var degree = ConvertDegreeDto(degreeDto);
            _context.Degrees.Add(degree);

            try
            {
                await _context.SaveChangesAsync();
                return CreatedAtAction("GetDegreeById", new { degreeId = degree.DegreeId });
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPut("degrees/edit/{degreeId}")]
        public async Task<ActionResult<Degree>> UpdateDegree(int degreeId, Degree degree)
        {
            if (degreeId != degree.DegreeId)
            {
                return BadRequest();
            }

            _context.Entry(degree).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DegreeExists(degreeId))
                {
                    return BadRequest();
                }
                else
                {
                    throw;
                }
            }

            return Ok(degree);
        }

        [HttpDelete("degrees/delete/{degreeId}")]
        public async Task<ActionResult> DeleteDegree(int degreeId)
        {
            var degree = await _context.Degrees.FirstOrDefaultAsync(degree => degree.DegreeId == degreeId);

            if (degree is null)
            {
                return BadRequest();
            }

            _context.Degrees.Remove(degree);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DegreeExists(int degreeId)
        {
            return _context.Degrees.Any(degree => degree.DegreeId == degreeId);
        }

        private Degree ConvertDegreeDto(DegreeDto degreeDto)
        {
            var faculty = _context.Faculties.FirstOrDefault(faculty => faculty.FacultyId == degreeDto.FacultyId);

            return new Degree(degreeDto.Name, faculty);
        }
    }
}
