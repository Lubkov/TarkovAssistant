using System.Collections.ObjectModel;
using TarkovAssistant.App.Models;

namespace TarkovAssistant.App.Services
{
    public static class TestDataService
    {
        public static MapModel GenerateMap()
        {
            return new MapModel()
            {
                Id = 2,
                Name = "Таможня",
                Left = 800,
                Top = -300,
                Right = -400,
                Bottom = 300
            };
        }
        public static ObservableCollection<MarkerGroupModel> GenerateQuestRepository()
        {
            var quests = new ObservableCollection<MarkerGroupModel>()
            {
                new MarkerGroupModel() { Id = 1, Name = "Врачебная тайна - Часть 3" },
                new MarkerGroupModel() { Id = 4, Name = "Поисковая миссия" },
                new MarkerGroupModel() { Id = 12, Name = "Операция \"Водолей\" - Часть 1", IsSelected = true },
                new MarkerGroupModel() { Id = 14, Name = "Проверка на вшивость" },
                new MarkerGroupModel() { Id = 112, Name = "Нефтянка" },
                new MarkerGroupModel() { Id = 113, Name = "Тряхнуть кассира" }
            };

            return quests;
        }

        public static ObservableCollection<MarkerGroupModel> GenerateExtractionRepository()
        {
            var items = new ObservableCollection<MarkerGroupModel>()
            {
                new MarkerGroupModel()
                {
                    Id = 1,
                    Name = "Выход ЧВК (3)",
                    Icon = @"/Resources/Images/map_pmc_extract.png",
                    Markers = new ()
                    {
                        new MarkerModel { Id = 36, Name = "ЗБ-1011", Left = 100, Top = 335 },
                        new MarkerModel { Id = 37, Name = "Старая заправка", Left = 340, Top = 285 },
                        new MarkerModel { Id = 38, Name = "ЗБ-013", Left = 200, Top = 335 }
                    }
                },
                new MarkerGroupModel()
                {
                    Id = 2,
                    Name = "Выходы дикого (1)",
                    Icon = @"/Resources/Images/map_scav_extract.png", 
                    IsSelected = true,
                    Markers = new ()
                    {
                        new MarkerModel { Id = 37, Name = "Старая заправка", Left = 300, Top = 235 }
                    }
                },
                new MarkerGroupModel()
                {
                    Id = 3,                    
                    Name = "Совм. выходы (0)",
                    Icon = @"/Resources/Images/map_coop_extract.png",
                    Markers = new ()
                },
                new MarkerGroupModel()
                {
                    Id = 4,
                    Name = "Переходы (0)",
                    Icon = @"/Resources/Images/map_transit_extract.png",
                    Markers = new ()
                }
            };

            return items;
        }
    }
}
