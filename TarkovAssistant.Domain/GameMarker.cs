using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TarkovAssistant.Domain
{
    [Table("Marker", Schema = "dbo")]
    public class GameMarker
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(256)]
        public string? Description { get; set; }

        [Required]
        public MarkerKind Kind { get; set; }

        public int Left { get; set; }

        public int Top { get; set; }

        public GameMap? Map { get; set; }

        public int? MapId { get; set; }

        public GameQuest? Quest { get; set; }

        public int? QuestId { get; set; }

        //public List<ResourceInfo> Resources { get; set; } = new();

        //public List<PictureInfo> Pictures { get; set; } = new();

        //[NotMapped]
        //public List<ResourceInfo> ScreenShots
        //{
        //    get
        //    {
        //        return Resources.Where(res => res.Kind == ResourceKind.Screenshot).ToList();
        //    }
        //}

        //[NotMapped]
        //public List<ResourceInfo> QuestItems
        //{
        //    get
        //    {
        //        return Resources.Where(res => res.Kind == ResourceKind.Quest).ToList();
        //    }
        //}

        public GameMarker()
        {

        }
    }
}
