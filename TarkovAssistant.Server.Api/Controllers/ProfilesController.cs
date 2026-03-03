using Microsoft.AspNetCore.Mvc;
using TarkovAssistant.Contracts;
using TarkovAssistant.Server.Services;

namespace TarkovAssistant.Server.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProfilesController : ControllerBase
    {
        private readonly ILogger<MapsController> _logger;
        private readonly IProfileService _profileService;

        public ProfilesController(ILogger<MapsController> logger, IProfileService profileService)
        {
            _logger = logger;
            _profileService = profileService;
        }

        [HttpGet(Name = "GetProfiles")]
        [ProducesResponseType(typeof(IEnumerable<ProfileDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetProfiles()
        {
            var items = await _profileService.GetProfilesAsync();
            var results = items.Select(p => new ProfileDto(p));

            return Ok(results);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(ProfileDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetProfileAsync([FromRoute] int id)
        {
            var result = await _profileService.GetProfileByIdAsync(id);
            
            if (result == null)
                return NotFound();

            return Ok(new ProfileDto(result));
        }
    }
}
