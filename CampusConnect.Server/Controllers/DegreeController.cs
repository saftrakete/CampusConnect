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
        public DegreeController(InitDegreeTable init)
        {
            
        }
    }
}
