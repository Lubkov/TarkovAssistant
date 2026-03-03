using TarkovAssistant.Domain;

namespace TarkovAssistant.Contracts
{
    public class QuestDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public TraderKind Trader { get; set; }

        public QuestDto()
        {
            Id = 0;
            Name = string.Empty;
            Trader = TraderKind.None;
        }
        public QuestDto(QuestEntity quest)
        {
            Id = quest.Id;
            Name = quest.Name;
            Trader = quest.Trader;
        }
    }
}
