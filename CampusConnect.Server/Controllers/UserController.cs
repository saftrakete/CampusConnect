using CampusConnect.Server.Data;
using Microsoft.AspNetCore.Mvc;

namespace CampusConnect.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly CampusConnectContext _context;

        public UserController(CampusConnectContext context)
        {
            _context = context;
        }
    }
}
