using IdentityService.Application.Responses;
using MediatR;

namespace IdentityService.Application.Commands
{
    public class UserRegistrationCommand : IRequest<UserRegistrationResponse>
    {
        public string UserName { get; }
        public string Email { get; }
        public string Password { get; }

        public UserRegistrationCommand(
            string userName,
            string email,
            string password)
        {
            UserName = userName;
            Email = email;
            Password = password;
        }
    }
}
