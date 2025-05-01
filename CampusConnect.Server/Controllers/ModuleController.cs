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
        private readonly CampusConnectContext _context;
        private readonly ILogger<ModuleController> _logger;
        private readonly InitDB _init;


        public ModuleController
        (
            CampusConnectContext context, 
            ILogger<ModuleController> logger,
            InitDB init)
        {
            _context = context;
            _logger = logger;
            _init = init;
        }

        // Füllt Modul-Tabelle mit einigen Testmodulen auf
        public void InitModuleTable()
        {
            this._init.FillInModules();
        }

    }
}