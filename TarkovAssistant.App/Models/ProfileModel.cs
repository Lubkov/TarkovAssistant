using CommunityToolkit.Mvvm.ComponentModel;
using TarkovAssistant.Domain;

namespace TarkovAssistant.App.Models
{
    public partial class ProfileModel : ObservableObject
    {
        [ObservableProperty]
        private int _id;

        [ObservableProperty]
        private string _name = string.Empty;

        [ObservableProperty]
        private ProfileKind _kind;

        public ProfileModel(ProfileEntity prifile)
        {
            Id = prifile.Id;
            Name = prifile.Name;
            Kind = prifile.Kind;
        }

        //public ProfileModel(Profile prifile)
        //{
        //    Id = prifile.Id;
        //    Name = prifile.Name;
        //    Kind = prifile.Kind;
        //}

        public ProfileModel()
        { 

        }

        //public static async Task<List<ProfileModel>> GetProfilesAsync()
        //{
        //    var filename = Path.Combine(AppContext.BaseDirectory, "profiles.json");
        //    using var stream = File.Open(filename, FileMode.Open, FileAccess.Read, FileShare.Read);

        //    return await JsonSerializer.DeserializeAsync<List<ProfileModel>>(stream) ?? [];
        //}

        //public static void SaveProfiles(Collection<ProfileModel> items)
        //{
        //    string json = JsonSerializer.Serialize(items, new JsonSerializerOptions { WriteIndented = true });
        //    File.WriteAllText(_filename, json);
        //}
    }
}
