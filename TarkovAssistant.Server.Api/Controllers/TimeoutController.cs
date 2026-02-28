using Microsoft.AspNetCore.Mvc;

namespace TarkovAssistant.Server.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TimeoutController : ControllerBase
    {
        private readonly ILogger<TimeoutController> _logger;

        public TimeoutController(ILogger<TimeoutController> logger)
        {
            _logger = logger;
        }

        [HttpGet("delay")]
        public async Task<IActionResult> Delay([FromQuery] int timeout)
        {
            if (timeout < 0 || timeout > 300)
                return BadRequest("Timeout must be between 0 and 300 seconds");

            await Task.Delay(timeout * 1000);

            return Ok($"Response after {timeout} seconds");
        }
    }
}
