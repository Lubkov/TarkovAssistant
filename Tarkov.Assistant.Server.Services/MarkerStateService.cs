using Microsoft.EntityFrameworkCore;
using TarkovAssistant.Data;
using TarkovAssistant.Domain;

namespace TarkovAssistant.Server.Services
{
    public class MarkerStateService : IMarkerStateService
    {
        private readonly ApplicationDbContext _context;

        public MarkerStateService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<List<MarkerStateEntity>> GetMarkerStatesAsync()
        {
            return await _context.MarkerStates.ToListAsync();
        }

        public async Task<MarkerStateEntity?> GetByKeysAsync(int profileId, int markerId)
        {
            return await _context.MarkerStates
                .Where(ms => ms.ProfileId == profileId && ms.MarkerId == markerId)
                .FirstOrDefaultAsync();
        }

        public async Task AddAsync(MarkerStateEntity state)
        {
            await _context.AddAsync(state);
            await _context.SaveChangesAsync();
        }

        public async Task AddAsync(int profileId, int markerId, bool isSeleced, bool isFinished)
        {
            await AddAsync(new MarkerStateEntity()
            {
                ProfileId = profileId,
                MarkerId = markerId,
                IsSeleced = isSeleced,
                IsFinished = isFinished
            });
        }

        public async Task UpdateAsync(MarkerStateEntity state)
        {
            await UpdateAsync(state.ProfileId, state.MarkerId, state.IsSeleced, state.IsFinished);
        }

        public async Task UpdateAsync(int profileId, int markerId, bool isSeleced, bool isFinished)
        {
            await _context.MarkerStates
                    .Where(ms => ms.ProfileId == profileId && ms.MarkerId == markerId)
                    .ExecuteUpdateAsync(s => s
                        .SetProperty(ms => ms.IsSeleced, isSeleced)
                        .SetProperty(ms => ms.IsFinished, isFinished));
        }

        public async Task SaveAsync(int profileId, int markerId, bool isSeleced, bool isFinished)
        {
            var stored = await _context.MarkerStates
                .Where(ms => ms.MarkerId == markerId && ms.ProfileId == profileId)
                .FirstOrDefaultAsync();

            if (stored != null)
            {
                await UpdateAsync(profileId, markerId, isSeleced, isFinished);
            }
            else
            {
                await AddAsync(profileId, markerId, isSeleced, isFinished);
            }
        }

        public async Task DeleteAsync(int profileId, int markerId)
        {
            await _context.MarkerStates
                .Where(ms => ms.ProfileId == profileId && ms.MarkerId == markerId)
                .ExecuteDeleteAsync();
        }
    }
}
