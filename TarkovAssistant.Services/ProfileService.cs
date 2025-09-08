using Microsoft.EntityFrameworkCore;
using TarkovAssistant.Data;
using TarkovAssistant.Domain;

namespace TarkovAssistant.Services
{
    public class ProfileService : IProfileService
    {
        private readonly ApplicationDbContext _context;

        public ProfileService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<ProfileEntity>> GetProfilesAsync()
        { 
            return await _context.Profiles.ToListAsync();
        }

        public async Task AddProfileAsync(ProfileEntity profile)
        {
            await _context.AddAsync(profile);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateProfileAsync(ProfileEntity profile)
        {
            await _context.Profiles
                .Where(p => p.Id == profile.Id)
                .ExecuteUpdateAsync(s => s
                    .SetProperty(p => p.Name, profile.Name)
                    .SetProperty(p => p.Kind, profile.Kind));
        }

        public async Task DeleteProfileAsync(int id)
        { 
            await _context.Profiles
                .Where(p => p.Id == id)
                .ExecuteDeleteAsync();
        }
    }
}
