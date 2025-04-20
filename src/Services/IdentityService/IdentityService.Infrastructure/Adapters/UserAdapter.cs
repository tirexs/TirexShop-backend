using IdentityService.Domain.Entities;

namespace IdentityService.Infrastructure.Adapters
{
    public static class UserAdapter
    {
        public static User ToDomainUser(this ApplicationUser identityUser, IList<string> roles)
        {
            return new User(
                id: identityUser.Id,
                email: identityUser.Email,
                userName: identityUser.UserName,
                roles: roles.ToList()
            );
        }
    }
}
