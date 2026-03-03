using Microsoft.EntityFrameworkCore;
using TarkovAssistant.Data;
using TarkovAssistant.Domain;

namespace TarkovAssistant.Server.Services
{
    public class MapService : IMapService
    {
        private readonly ApplicationDbContext _dbContext;

        public MapService(ApplicationDbContext context)
        {
            this._dbContext = context;
        }

        public async Task<MapEntity?> GetMapByIdAsync(int mapId, int? profileId)
        {
            var query = _dbContext.Maps
                //.AsNoTracking()
                .Include(map => map.Layers)
                    .ThenInclude(layer => layer!.Resource)
                .Include(map => map.Markers)
                    .ThenInclude(marker => marker!.Quest)
                .Where(map => map.Id == mapId);

            if (profileId != null)
            {
                query = query.Include(map => map.Markers)
                    .ThenInclude(marker => marker.MarkerStates
                        .Where(state => state.ProfileId == profileId));
            }

            return await query.FirstOrDefaultAsync();
        }

        public async Task<MapEntity?> GetMapByIdAsync(int mapId)
        {
            return await _dbContext.Maps
                //.AsNoTracking()
                .Include(map => map.Layers)
                    .ThenInclude(layer => layer!.Resource)
                .Include(map => map.Markers)
                    .ThenInclude(marker => marker!.Quest)
                .Where(map => map.Id == mapId).FirstOrDefaultAsync();
        }

        public async Task<List<MapEntity>> GetMapsAsync()
        {
            return await _dbContext.Maps
                .AsNoTracking()
                .Include(map => map.Resource)
                .ToListAsync();
        }

        public async Task<List<LayerEntity>> GetLayersForMapAsync(int mapId)
        {
            return await _dbContext.Layers
                .AsNoTracking()
                .Where(layer => layer.MapId == mapId)
                .ToListAsync();
        }

        public async Task LoadLayersForMapAsync(MapEntity map)
        {
            if (map == null)
                throw new ArgumentNullException(nameof(map));

            await _dbContext.Entry(map)
                .Collection(m => m.Layers)
                .LoadAsync();
        }

        public async Task<List<MarkerEntity>> GetMarkersForMapAsync(int mapId)
        {
            return await GetMarkersForMapAsync(mapId, null);
        }

        public async Task<List<MarkerEntity>> GetMarkersForMapAsync(int mapId, int? profileId)
        {
            var query = _dbContext.Markers
                .AsNoTracking()
                .Where(marker => marker.MapId == mapId);

            if (profileId != null)
            {
                query = query.Include(marker => marker.MarkerStates
                    .Where(state => state.ProfileId == profileId));
            }

            return await query.ToListAsync();
        }

        public async Task<List<QuestEntity>> GetQuestsForMapAsync(int mapId)
        {
            return await _dbContext.Quests
                .AsNoTracking()
                //.Include(q => q.Markers)
                .Where(q => q.Markers.Any(m => m.MapId == mapId))
                .ToListAsync();
        }
    }
}
