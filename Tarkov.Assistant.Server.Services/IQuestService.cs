using TarkovAssistant.Domain;

namespace TarkovAssistant.Server.Services
{
    public interface IQuestService
    {
        Task<List<QuestEntity>> GetQuestsAsync();
    }
}
