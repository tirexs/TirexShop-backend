using ProfileService.Domain.Entities;

namespace ProfileService.Domain.Interfaces.Repositories;

public interface IProfileRepository
{
    Task AddAsync(Profile profile, CancellationToken cancellationToken = default);
    Task<bool> ExistsByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task SaveAsync(CancellationToken cancellationToken = default);
}