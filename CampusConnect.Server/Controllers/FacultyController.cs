using System.Security.Cryptography;
using CampusConnect.Server.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CampusConnect.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FacultyController : ControllerBase {
        private readonly InitFacultyTable _init;
        public FacultyController(InitFacultyTable init) 
        {
            this._init = init;
        }

        public void InitFacultyTable()
        {
            this._init.FillInFaculties();
        }
    }
}
