using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using TarkovAssistant.App.Messages;

namespace TarkovAssistant.App.ViewModels
{
    public partial class MessageWindowViewModel : ObservableObject
    {
        [ObservableProperty]
        private string _title;

        [ObservableProperty]
        private string _message;

        public MessageWindowViewModel(string title, string message)
        { 
            _title = title;
            _message = message;
        }

        [RelayCommand]
        private void Confirm()
        {
            WeakReferenceMessenger.Default.Send(new CloseDialogMessage(true));
        }

        [RelayCommand]
        private void Cancel()
        {
            WeakReferenceMessenger.Default.Send(new CloseDialogMessage(false));
        }
    }
}
