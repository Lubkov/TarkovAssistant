using Microsoft.EntityFrameworkCore;
using TarkovAssistant.Data;
using TarkovAssistant.Domain;

namespace TarkovAssistant.Server.Services
{
    public class MarkerService : IMarkerService
    {
        private readonly ApplicationDbContext _dbContext;

        public MarkerService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<MarkerEntity>> GetMarkersAsync()
        {
            return await _dbContext.Markers.ToListAsync();
        }

        public async Task<MarkerEntity?> GetMarkerByIdAsync(int id, int? profileId)
        {
            var query = _dbContext.Markers
                .AsNoTracking()
                .Include(m => m.Quest)
                .Include(m => m.Resources)
                .Where(m => m.Id == id);

            if (profileId != null)
            {
                query = query.Include(marker => marker.MarkerStates
                    .Where(state => state.ProfileId == profileId));
            }

            return await query.FirstOrDefaultAsync();
        }
    }
}
