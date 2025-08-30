using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TarkovAssistant.Domain
{
    [Table("Layer", Schema = "dbo")]
    public class GameLayer
    {
        [Key]
        public int Id { get; set; }

        public LayerLevel Level { get; set; }

        [Required]
        [MaxLength(64)]
        public string? Name { get; set; }

        public byte[]? Picture { get; set; }

        public GameMap? Map { get; set; }

        public int? MapId { get; set; }

        public bool IsMainLayer()
        {   
            return Level == LayerLevel.Main;
        }
    }
}
