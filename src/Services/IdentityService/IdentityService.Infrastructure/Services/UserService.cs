using IdentityService.Domain.Entities;
using IdentityService.Domain.Interfaces;
using IdentityService.Infrastructure.Adapters;
using Microsoft.AspNetCore.Identity;

namespace IdentityService.Infrastructure.Services
{
    public class UserService : IUserService
    {
        #region private
        private readonly UserManager<ApplicationUser> _userManager;
        #endregion

        public UserService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task UserRegistrationAsync(string username, string email, string password, CancellationToken cancellationToken)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                var user = new ApplicationUser
                {
                    UserName = username,
                    Email = email
                };

                var result = await _userManager.CreateAsync(user, password);
                if (!result.Succeeded)
                {
                    throw new Exception($"{result.Errors.Aggregate("Text:", (first, next) => $"{first} {next.Description}")}");
                }
            }
            catch (OperationCanceledException ex) { throw; }
        }

        public async Task<User> UserAuthorizationAsync(string username, string password, CancellationToken cancellationToken)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                var user = await _userManager.FindByNameAsync(username);
                if (user == null)
                {
                    throw new Exception("Пользователь не существует");
                }
                else if (!await _userManager.CheckPasswordAsync(user, password))
                {
                    throw new Exception("Неверный пароль");
                }

                var roles = await _userManager.GetRolesAsync(user);

                return user.ToDomainUser(roles);
            }
            catch (OperationCanceledException ex) { throw; }
        }
    }
}
