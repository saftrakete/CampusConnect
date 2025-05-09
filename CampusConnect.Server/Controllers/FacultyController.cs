using System.Security.Cryptography;
using System.Threading.Tasks;
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

        public Task InitFacultyTable()
        {
            return this._init.FillInFaculties();
        }
    }
}
