using CampusConnect.Server.Data;
using CampusConnect.Server.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CampusConnect.Server.Services;

namespace CampusConnect.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ModuleController : ControllerBase
    {
        private readonly ILogger<ModuleController> _logger;
        private readonly InitModuleTable _init;


        public ModuleController
        ( 
            ILogger<ModuleController> logger,
            InitModuleTable init)
        {
            _logger = logger;
            _init = init;
        }

        // Füllt Modul-Tabelle mit einigen Testmodulen auf
        public Task InitModuleTable()
        {
            return this._init.FillInModules();
        }

    }
}