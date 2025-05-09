using CampusConnect.Server.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CampusConnect.Server.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class DegreeController : ControllerBase
    {
        private readonly InitDegreeTable _init;

        public DegreeController(InitDegreeTable init)
        {
            this._init = init;
        }

        public Task InitDegreeTable()
        {
            return this._init.FillInDegrees();
        }
    }
}
