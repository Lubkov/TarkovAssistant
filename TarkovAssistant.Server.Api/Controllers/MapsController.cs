using Microsoft.AspNetCore.Mvc;
using TarkovAssistant.Contracts;
using TarkovAssistant.Domain;
using TarkovAssistant.Server.Services;

namespace TarkovAssistant.Server.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MapsController : ControllerBase
    {
        private readonly ILogger<MapsController> _logger;
        private readonly IMapService _mapService;

        public MapsController(ILogger<MapsController> logger, IMapService mapService)
        {
            _logger = logger;
            _mapService = mapService;
        }

        [HttpGet(Name = "GetMaps")]
        [ProducesResponseType(typeof(IEnumerable<MapDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetMaps()
        {
            var items = await _mapService.GetMapsAsync();
            var results = items.Select(m => new MapDto(m));

            return Ok(results);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(MapFullDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetMapAsync([FromRoute]int id, [FromQuery] int? profileId)
        {
            var map = profileId is null
                ? await _mapService.GetMapByIdAsync(id)
                : await _mapService.GetMapByIdAsync(id, profileId);

            if (map == null)
                return NotFound();

            return Ok(new MapFullDto(map));
        }

        [HttpGet("{id:int}/quests")]
        [ProducesResponseType(typeof(IEnumerable<QuestDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetQuestsAsync([FromRoute] int id)
        {
            var items = await _mapService.GetQuestsForMapAsync(id);
            var results = items.Select(quest => new QuestDto(quest));

            return Ok(results);
        }
    }
}
