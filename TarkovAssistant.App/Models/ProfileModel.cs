using CommunityToolkit.Mvvm.ComponentModel;
using TarkovAssistant.Contracts;
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

        public ProfileModel(ProfileDto prifile)
        {
            Id = prifile.Id;
            Name = prifile.Name;
            Kind = prifile.Kind;
        }

        public ProfileModel()
        { 
        }
    }
}
