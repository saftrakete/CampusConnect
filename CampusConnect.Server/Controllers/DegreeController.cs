using CampusConnect.Server.Data;
using CampusConnect.Server.Models;
using CampusConnect.Server.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
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
            this._context = context;
        }

        [HttpGet("onboarding/{degreeName}")]
        public async Task<ActionResult<ICollection<ModuleDto>>> GetModulesForDegree(string degreeName)
        {
            var degree = await _context.Degrees.Include(deg => deg.MandatoryModules).FirstOrDefaultAsync(deg => deg.Name == degreeName);

            if (degree == null)
            {
                return NotFound("Degree not found.");
            }

            // Nur die nötigen Daten für das Frontend bereitstellen
            var result = degree.MandatoryModules.Select(mod => new ModuleDto
            {   
                ModuleId = mod.ModuleId,
                Name = mod.Name
            }).ToList();

            return result;
        }
    }
}
