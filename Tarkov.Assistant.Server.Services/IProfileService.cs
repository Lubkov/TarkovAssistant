using TarkovAssistant.Domain;

namespace TarkovAssistant.Server.Services
{
    public interface IProfileService
    {
        Task<List<ProfileEntity>> GetProfilesAsync();
        Task<ProfileEntity?> GetProfileByIdAsync(int id);
        Task AddProfileAsync(ProfileEntity profile);
        Task UpdateProfileAsync(ProfileEntity profile);
        Task DeleteProfileAsync(int id);
    }
}
