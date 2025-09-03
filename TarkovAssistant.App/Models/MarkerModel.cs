using TarkovAssistant.App.Localization;
using TarkovAssistant.Domain;

namespace TarkovAssistant.App.Models
{
    public partial class MarkerModel : PositionModel
    {
        public int Id { get; set; }

        public string? Description { get; set; }

        public MarkerKind Kind { get; set; }

        public string Picture { get; set; }

        public int? MapId { get; set; }

        public int? QuestId { get; set; }        

        public MarkerModel(GameMarker marker) : base(marker.Top, marker.Left, 0)
        { 
            Id = marker.Id;
            Description = marker.Description;
            Kind = marker.Kind;
            MapId = marker.MapId;
            QuestId = marker.QuestId;
            Picture = GetMarkerIcon(marker.Kind);
            IsVisibile = false;            
        }               

        public static string GetMarkerIcon(MarkerKind kind)
        {
            const string path = @"/Resources/Images/";

            switch (kind)
            {
                case MarkerKind.PMCExtraction:
                    return path + "map_pmc_extract.png";
                case MarkerKind.ScavExtraction:
                    return path + "map_scav_extract.png";
                case MarkerKind.CoopExtraction:
                    return path + "map_coop_extract.png";
                case MarkerKind.TransitExtraction:
                    return path + "map_transit_extract.png";
                case MarkerKind.Quest:
                    return path + "map_quest.png";
           }

            return string.Empty;
        }

        public static string GetMarkerKindName(MarkerKind kind)
        {
            switch (kind)
            {
                case MarkerKind.PMCExtraction:
                    return Labels.PMCExtraction_Name;
                case MarkerKind.ScavExtraction:
                    return Labels.ScavExtraction_Name;
                case MarkerKind.CoopExtraction:
                    return Labels.CoopExtraction_Name;
                case MarkerKind.TransitExtraction:
                    return Labels.TransitExtraction_Name;
                case MarkerKind.Quest:
                    return Labels.Quest_Name;
            }

            return string.Empty;
        }        
    }
}
