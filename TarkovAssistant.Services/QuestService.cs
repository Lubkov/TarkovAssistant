using Microsoft.EntityFrameworkCore;
using TarkovAssistant.Data;
using TarkovAssistant.Domain;

namespace TarkovAssistant.Services
{
    public class QuestService : IQuestService
    {

        private readonly ApplicationDbContext _db;

        public QuestService(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<List<GameQuest>> GetQuestsAsync()
        {
            return await _db.Quests.ToListAsync();
        }
    }
}
