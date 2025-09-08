using TarkovAssistant.Domain;

namespace TarkovAssistant.Services
{
    public interface IProfileService
    {
        Task<List<ProfileEntity>> GetProfilesAsync();
        Task AddProfileAsync(ProfileEntity profile);
        Task UpdateProfileAsync(ProfileEntity profile);
        Task DeleteProfileAsync(int id);
    }
}
