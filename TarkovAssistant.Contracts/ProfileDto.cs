using TarkovAssistant.Domain;

namespace TarkovAssistant.Contracts
{
    public class ProfileDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ProfileKind Kind { get; set; }

        public ProfileDto()
        {
            Id = 0;
            Name = string.Empty;
            Kind = ProfileKind.Bear;
        }

        public ProfileDto(ProfileEntity profile)
        {
            Id = profile.Id;
            Name = profile.Name;
            Kind = profile.Kind;
        }
    }
}
