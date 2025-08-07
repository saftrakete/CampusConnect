using Microsoft.AspNetCore.Mvc;
using CampusConnect.Server.Data;
using CampusConnect.Server.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace CampusConnect.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DiagnosticController : ControllerBase
    {
        private readonly CampusConnectContext _context;
        private readonly TwoFactorService _twoFactorService;

        public DiagnosticController(CampusConnectContext context, TwoFactorService twoFactorService)
        {
            _context = context;
            _twoFactorService = twoFactorService;
        }

        [HttpGet("db-test")]
        public async Task<IActionResult> TestDatabase()
        {
            try
            {
                var canConnect = await _context.Database.CanConnectAsync();
                var userCount = await _context.Users.CountAsync();
                
                return Ok(new { 
                    CanConnect = canConnect, 
                    UserCount = userCount,
                    ConnectionString = _context.Database.GetConnectionString()
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message, StackTrace = ex.StackTrace });
            }
        }

        [HttpGet("totp-test/{secret}/{code}")]
        public IActionResult TestTOTP(string secret, string code)
        {
            try
            {
                var isValid = _twoFactorService.ValidateCode(secret, code);
                var serverTime = DateTime.UtcNow;
                
                return Ok(new { 
                    IsValid = isValid, 
                    ServerTime = serverTime,
                    ServerTimeUnix = ((DateTimeOffset)serverTime).ToUnixTimeSeconds(),
                    Secret = secret,
                    Code = code
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }
    }
}