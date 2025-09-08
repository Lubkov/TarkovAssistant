using TarkovAssistant.Domain;

namespace TarkovAssistant.Services
{
    public interface IQuestService
    {
        Task<List<QuestEntity>> GetQuestsAsync();
    }
}
