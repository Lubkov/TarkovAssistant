using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using TarkovAssistant.Services;

namespace TarkovAssistant.App.Models
{
    public partial class OptionsModel : ObservableObject
    {
        private readonly IAppService _appService;
        private readonly IWebApiService _webApiService;

        [ObservableProperty]
        private string _dataPath;
        
        partial void OnDataPathChanged(string value)
        {
            DoOptionsChanged();
        }
        
        [ObservableProperty]
        private string _sreenshotPath;

        partial void OnSreenshotPathChanged(string value)
        {
            DoOptionsChanged();
        }

        [ObservableProperty]
        private bool _trackLocation;

        partial void OnTrackLocationChanged(bool value)
        {
            DoOptionsChanged();
        }

        [ObservableProperty]
        private ProfileModel? _currentProfile;

        partial void OnCurrentProfileChanged(ProfileModel? value)
        {
            DoOptionsChanged();
        }

        [ObservableProperty]
        private ObservableCollection<ProfileModel> _profiles = [];

        partial void OnProfilesChanged(ObservableCollection<ProfileModel> value)
        {
            DoOptionsChanged();
        }

        public event EventHandler? OptionsChanged;

        public OptionsModel(IAppService appService, IWebApiService webApiService)
        {
            _appService = appService;
            _webApiService = webApiService;
            _dataPath = ""; // appService.Options.DataPath;
            _sreenshotPath = appService.Options.SreenshotPath;
            _trackLocation = appService.Options.TrackLocation;

            _ = LoadProfiles();
        }

        public async Task LoadProfiles()
        {
            var items = await _webApiService.GetProfilesAsync();
            foreach (var profile in items)
            {
                Profiles.Add(new ProfileModel(profile));
            }

            CurrentProfile = Profiles.Where(p => p.Id == _appService.Options.Profile).FirstOrDefault();
        }

        public void AppllyTo(AppOptions options)
        {
            //options.DataPath = DataPath;
            options.SreenshotPath = SreenshotPath;
            options.TrackLocation = TrackLocation;
            options.Profile = CurrentProfile?.Id ?? null;
        }

        private void DoOptionsChanged()
        {
            OptionsChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
