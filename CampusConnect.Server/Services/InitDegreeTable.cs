using CampusConnect.Server.Models;
using CampusConnect.Server.Data;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Data.SqlTypes;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace CampusConnect.Server.Services
{
    public class InitDegreeTable
    {
        private Degree[] degrees;
        private readonly int[] infModuleIds = Enumerable.Range(1, 9).ToArray();
        private readonly int[] ingInfModuleIds = Enumerable.Range(1, 8).Append(10).ToArray();
        private readonly CampusConnectContext _context;

        public InitDegreeTable(CampusConnectContext context)
        {
            this._context = context;
        }

        public void CreateDegreeEntities()
        {
            this.degrees = [
                    new Degree { Name = "Informatik", Faculty = _context.Faculties.First(), MandatoryModules = GetModulesById(infModuleIds) },
                    new Degree { Name = "Ingenieurinformatik", Faculty = _context.Faculties.First(), MandatoryModules = GetModulesById(ingInfModuleIds) }
                ];  
        }

        // Kann wahrscheinlich raus sobald wir ein Admin-Dashboard zum Bearbeiten von DB-Tabellen haben
        public void DeleteTableContent()
        {
            _context.Database.ExecuteSqlRaw("DELETE FROM Degrees");
            _context.Database.ExecuteSqlRaw("DBCC CHECKIDENT ('Degrees', RESEED, 0)");
        }

        public ICollection<Module> GetModulesById(int[] modIds)
        {
            return _context.Modules.Where(mod => modIds.Contains(mod.ModuleId)).ToList();
        }

        public async Task FillInDegrees()
        {
            CreateDegreeEntities();

            if (CheckIfEmpty())
            {
                foreach (var deg in degrees)
                {
                    await _context.Degrees.AddAsync(deg);
                }
                await _context.SaveChangesAsync();
            }
        }

        public bool CheckIfEmpty()
        {
            int degreeCount = _context.Degrees.Count();
            return degreeCount == 0;
        }
    }
}
