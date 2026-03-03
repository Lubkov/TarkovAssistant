using Microsoft.AspNetCore.Mvc;
using TarkovAssistant.Contracts;
using TarkovAssistant.Server.Services;

namespace TarkovAssistant.Server.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MarkersController : ControllerBase
    {
        private readonly ILogger<MapsController> _logger;
        private readonly IMarkerService _markerService;
        private readonly IMarkerStateService _markerStateService;        

        public MarkersController(ILogger<MapsController> logger, IMarkerService markerService, IMarkerStateService markerStateService)
        {
            _logger = logger;
            _markerService = markerService;
            _markerStateService = markerStateService;
        }

        [HttpPost("state")]
        public async Task<IActionResult> SaveMarkerState([FromBody] MarkerStateDto state)
        {
            if (state == null)
                return BadRequest();

            await _markerStateService.SaveAsync(state.ProfileId, state.MarkerId, state.IsSeleced, state.IsFinished);

            return Ok(new { message = "User created", state });
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(MarkerFullDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetMarkerAsync([FromRoute] int id, [FromQuery] int? profileId)
        {
            var marker = await _markerService.GetMarkerByIdAsync(id, profileId);

            if (marker == null)
                return NotFound();

            return Ok(new MarkerFullDto(marker));
        }
    }
}
