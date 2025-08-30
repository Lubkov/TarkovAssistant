using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TarkovAssistant.Domain
{
    [Table("Map", Schema = "dbo")]
    public class GameMap
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(64)]
        public string Name { get; set; } = string.Empty;

        public int Left { get; set; }

        public int Top { get; set; }

        public int Right { get; set; }

        public int Bottom { get; set; }

        public byte[]? Picture { get; set; }

        public List<GameLayer> Layers { get; set; } = new();

        public GameMap()
        { 
        
        }
    }
}
