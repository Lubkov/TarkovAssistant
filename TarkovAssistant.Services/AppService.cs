using System.Text.Json;
using Microsoft.Extensions.Options;
using TarkovAssistant.Domain;

namespace TarkovAssistant.Services
{
    public class AppService : IAppService
    {
        private readonly AppOptions _options;
        private readonly string _settingsFileName;

        public AppOptions Options { get => _options; }

        public AppService(IOptions<AppOptions> options)
        {
            _options = options.Value;
            _settingsFileName = Path.Combine(AppContext.BaseDirectory, "appsettings.json");
        }

        public async Task AddProfileAsync(Profile profile)
        {
            //profile.Id = Guid.NewGuid();
            //Options.Profiles.Add(profile);
            //Options.Profile = profile.Id;

            await SaveOptionsAsync();
        }

        public async Task UpdateProfileAsync(Profile profile)
        {
            //var item = Options.Profiles.Where(p => p.Id == profile.Id).FirstOrDefault();
            //if (item != null)
            //{
            //    item.Name = profile.Name;
            //    item.Kind = profile.Kind;
            //    // Markers ??

            //    await SaveOptionsAsync();
            //}
        }

        public async Task RemoveProfileAsync(Guid id)
        {
            //var item = Options.Profiles.Where(p => p.Id == id).FirstOrDefault();
            //if ((item != null && Options.Profiles.Remove(item)) || Options.Profile != null)
            //{
            //    Options.Profile = null;
            //    await SaveOptionsAsync();
            //}
        }

        public async Task SaveMarkerStateAsync(Profile profile, MarkerState state)
        {
            var item = profile.Markers.Where(m => m.MarkerId == state.MarkerId).FirstOrDefault();
            if (item == null)
            {
                profile.Markers.Add(state);
            }
            else
            {
                item.IsSeleced = state.IsSeleced;
                item.IsFinished = state.IsFinished;
            }

            await SaveOptionsAsync();
        }

        public async Task SaveOptionsAsync()        
        {
            var json = File.Exists(_settingsFileName)
                ? await File.ReadAllTextAsync(_settingsFileName)
                : "{}";

            var document = JsonSerializer.Deserialize<Dictionary<string, object>>(json)
                           ?? new Dictionary<string, object>();

            document["Settings"] = _options;

            var options = JsonSerializer.Serialize(document, new JsonSerializerOptions
            {
                WriteIndented = true
            });

            await File.WriteAllTextAsync(_settingsFileName, options);
        }
    }
}
