using Microsoft.AspNetCore.Mvc;
using CampusConnect.Server.Services;
using System;

namespace CampusConnect.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        private readonly TwoFactorService _twoFactorService;

        public TestController(TwoFactorService twoFactorService)
        {
            _twoFactorService = twoFactorService;
        }

        [HttpGet("2fa-test")]
        public IActionResult Test2FA()
        {
            try
            {
                var secret = _twoFactorService.GenerateSecret();
                var uri = _twoFactorService.GenerateQrCodeUri(secret, "test@example.com");
                
                return Ok(new { Secret = secret, Uri = uri, Status = "Working" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }

        [HttpGet("ping")]
        public IActionResult Ping()
        {
            return Ok(new { Message = "Server is running", Time = DateTime.Now });
        }
    }
}