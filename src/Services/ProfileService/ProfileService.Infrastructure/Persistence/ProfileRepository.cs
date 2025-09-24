using Microsoft.EntityFrameworkCore;
using ProfileService.Domain.Entities;
using ProfileService.Domain.Interfaces.Repositories;

namespace ProfileService.Infrastructure.Persistence;

public sealed class ProfileRepository : IProfileRepository
{
    #region private
    private readonly ProfileDbContext _context;
    #endregion

    public ProfileRepository(ProfileDbContext context)
    {
        _context = context;
    }
    
    public async Task AddAsync(Profile profile, CancellationToken cancellationToken = default)
    {
        await _context.Profiles.AddAsync(profile, cancellationToken);
    }

    public async Task<bool> ExistsByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var user = await _context.Profiles.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        
        return user is not null;
    }

    public async Task SaveAsync(CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}