using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using TarkovAssistant.App.Messages;
using TarkovAssistant.Domain;

namespace TarkovAssistant.App.ViewModels
{
    public partial class ProfileWindowViewModel : ObservableObject
    {
        private ProfileEntity _profile;

        [ObservableProperty]
        private string _profileName;

        [ObservableProperty]
        private bool _isBear;

        [ObservableProperty]
        private bool _isUsec;

        public ProfileWindowViewModel(ProfileEntity profile) 
        {
            _profile = profile;

            _profileName = profile.Name;
            _isBear = profile.Kind == ProfileKind.Bear;
            _isUsec = profile.Kind == ProfileKind.Usec;
        }

        [RelayCommand]
        private void Save()
        {
            _profile.Name = ProfileName;
            if (IsBear)
            { 
                _profile.Kind = ProfileKind.Bear;
            }
            else
            {
                _profile.Kind = ProfileKind.Usec;
            }

                WeakReferenceMessenger.Default.Send(new CloseDialogMessage(true));
        }

        [RelayCommand]
        private void Cancel()
        {
            WeakReferenceMessenger.Default.Send(new CloseDialogMessage(false));
        }
    }
}
