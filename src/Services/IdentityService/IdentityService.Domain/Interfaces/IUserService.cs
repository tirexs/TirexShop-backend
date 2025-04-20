using IdentityService.Domain.Entities;

namespace IdentityService.Domain.Interfaces
{
    public interface IUserService
    {
        Task UserRegistrationAsync(string username, string email, string password, CancellationToken cancellationToken);
        Task<User> UserAuthorizationAsync(string username, string password, CancellationToken cancellationToken);
    }
}
