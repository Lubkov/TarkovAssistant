using Microsoft.AspNetCore.Mvc;
using TarkovAssistant.Contracts;
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
        [ProducesResponseType(typeof(IEnumerable<MapSummaryDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetMaps()
        {
            var items = await _mapService.GetMapsAsync();
            var results = items.Select(m => new MapSummaryDto(m));

            return Ok(results);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(MapFullDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetMapAsync([FromRoute]int id)
        {
            var result = await _mapService.GetMapByIdAsync(id);
            if (result == null)
                return NotFound();

            return Ok(new MapFullDto(result));
        }
    }
}
