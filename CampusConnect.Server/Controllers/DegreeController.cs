using CampusConnect.Server.Data;
using CampusConnect.Server.Models;
using CampusConnect.Server.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CampusConnect.Server.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class DegreeController : ControllerBase
    {
        private readonly CampusConnectContext _context;

        public DegreeController(CampusConnectContext context)
        {
            _context = context;
        }

        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<Degree>>> GetDegrees()
        {
            var result = await _context.Degrees.ToListAsync();

            return result is not null ? result : NotFound();
        }

        [HttpGet("{degreeId}")]
        public async Task<ActionResult<Degree>> GetDegreeById(int degreeId)
        {
            var degree = await _context.Degrees
                .Where(degree => degree.DegreeId == degreeId)
                .FirstOrDefaultAsync();

            return degree is not null ? degree : NotFound();
        }

        [HttpPost("postDegree")]
        public async Task<ActionResult<Degree>> PostNewDegree(Degree degree)
        {
            if (degree is null)
            {
                return BadRequest();
            }

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

        [HttpPut("update/{degreeId}")]
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

        [HttpDelete("delete/{degreeId}")]
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
    }
}
