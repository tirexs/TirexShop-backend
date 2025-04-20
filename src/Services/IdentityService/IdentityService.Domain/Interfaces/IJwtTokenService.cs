using IdentityService.Domain.Entities;

namespace IdentityService.Domain.Interfaces
{
    public interface IJwtTokenService
    {
        string GenerateJwtToken(User user);
    }
}
