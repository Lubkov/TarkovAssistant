using TarkovAssistant.Domain;

namespace TarkovAssistant.Services
{
    public interface IAppService
    {
        public AppOptions Options { get; }

        Task AddProfileAsync(Profile profile);
        Task UpdateProfileAsync(Profile profile);
        Task RemoveProfileAsync(Guid id);
        Task SaveMarkerStateAsync(Profile profile, MarkerState state);
        Task SaveOptionsAsync();
    }
}
