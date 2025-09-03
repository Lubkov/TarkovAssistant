using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TarkovAssistant.Domain
{
    [Table("Quest", Schema = "dbo")]
    public class GameQuest
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(64)]
        public string Name { get; set; }

        public GameTrader Trader { get; set; }

        public List<GameMarker> Markers { get; set; } = new();

        public GameQuest()
        {
            Name = "";
            Trader = GameTrader.None;
        }

        public GameQuest(string name, GameTrader trader)
        {
            Name = name;
            Trader = trader;
        }
    }
}
