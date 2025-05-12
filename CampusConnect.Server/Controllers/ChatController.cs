using CampusConnect.Server.Data;
using CampusConnect.Server.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CampusConnect.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ChatController : ControllerBase
    {
        private readonly CampusConnectContext _context;
        private readonly ILogger<ControllerBase> _logger;

        public ChatController(CampusConnectContext context, ILogger<ControllerBase> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet("{messageId}")]
        public async Task<ActionResult<MessageModel>> GetMessageById(int messageId)
        {
            var message = await _context.Messages.FirstOrDefaultAsync(m => m.MessageId == messageId);
            return message is not null ? Ok(message) : NotFound();
        }

        [HttpPost("post")]
        public async Task<IActionResult> PostNewMessage([FromBody] MessageModel model)
        {
            if (_context.Messages.Any(m => m.MessageId == model.MessageId))
            {
                return BadRequest("MessageId already exists.");
            }

            var message = new MessageModel
            {
                Content = model.Content,
                MessageId = model.MessageId
            };

            _context.Messages.Add(message);
            await _context.SaveChangesAsync();

            return Ok("Message posted successfully.");
        }
    }
}
