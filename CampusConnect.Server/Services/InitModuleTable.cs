using CampusConnect.Server.Controllers;
using CampusConnect.Server.Data;
using CampusConnect.Server.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;

namespace CampusConnect.Server.Services
{
    public class InitModuleTable {
        private readonly Module[] modules =
            [
                new Module { Name = "Mathe1"},
                new Module { Name = "Mathe2"},
                new Module { Name = "Mathe3"},
                new Module { Name = "Einführung Informatik"},
                new Module { Name = "Technische Informatik 1"},
                new Module { Name = "Technische Informatik 2"},
                new Module { Name = "Datenbanken 1"},
                new Module { Name = "Theoretische Informatik 1"},
                new Module { Name = "Theoretische Informatik 2"},
                new Module { Name = "Spezifikationstechnik"},
                new Module { Name = "Introduction to Simulation"},
                new Module { Name = "Programmierparadigmen"}
            ];
        private readonly CampusConnectContext _context;
        private readonly ILogger<InitModuleTable> _logger;

        public InitModuleTable(CampusConnectContext context, ILogger<InitModuleTable> logger) 
        {
            this._context = context;
            this._logger = logger;
        }

        public async void FillInModules()
        {
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
