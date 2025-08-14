using CampusConnect.Server.Controllers;
using CampusConnect.Server.Data;
using CampusConnect.Server.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace CampusConnect.Server.Services
{
    public class InitModuleTable {
        private Module[] modules = [];
        private readonly CampusConnectContext _context;
        private readonly ILogger<InitModuleTable> _logger;

        public InitModuleTable(CampusConnectContext context, ILogger<InitModuleTable> logger) 
        {
            this._context = context;
            this._logger = logger;
        }

        public void CreateModuleEntities()
        {
            this.modules = [
                    new Module { Name = "Mathe1", Semester = 1, Faculty = _context.Faculties.First()},
                    new Module { Name = "Mathe2", Semester = 2, Faculty = _context.Faculties.First()},
                    new Module { Name = "Mathe3", Semester = 3, Faculty = _context.Faculties.First()},
                    new Module { Name = "Einführung Informatik", Semester = 1, Faculty = _context.Faculties.First()},
                    new Module { Name = "Technische Informatik 1", Semester = 1, Faculty = _context.Faculties.First()},
                    new Module { Name = "Technische Informatik 2", Semester = 2, Faculty = _context.Faculties.First()},
                    new Module { Name = "Datenbanken 1", Semester = 1, Faculty = _context.Faculties.First()},
                    new Module { Name = "Theoretische Informatik 1", Semester = 3, Faculty = _context.Faculties.First()},
                    new Module { Name = "Theoretische Informatik 2", Semester = 4, Faculty = _context.Faculties.First()},
                    new Module { Name = "Spezifikationstechnik", Semester = 4, Faculty = _context.Faculties.First()},
                    new Module { Name = "Introduction to Simulation", Semester = 5, Faculty = _context.Faculties.First()},
                    new Module { Name = "Programmierparadigmen", Semester = 3, Faculty = _context.Faculties.First()}
                ];
            var mod = this.modules[0];
        }

        // Kann wahrscheinlich raus sobald wir ein Admin-Dashboard zum Bearbeiten von DB-Tabellen haben
        public void DeleteTableContent()
        {
            _context.Database.ExecuteSqlRaw("DELETE FROM Modules");
            _context.Database.ExecuteSqlRaw("DBCC CHECKIDENT ('Modules', RESEED, 0)");
        }

        public async Task FillInModules()
        {
            CreateModuleEntities();

            DeleteTableContent();

            if (CheckIfEmpty()) 
            {
                foreach (var mod in modules)
                {
                    await _context.Modules.AddAsync(mod);
                }
                await _context.SaveChangesAsync();
            }
        }

        public bool CheckIfEmpty()
        {
            int moduleCount = _context.Modules.Count();
            return moduleCount == 0;
        } 

    }


}
