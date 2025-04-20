using IdentityService.Application.Responses;
using MediatR;

namespace IdentityService.Application.Commands
{
    public class UserAuthorizationCommand : IRequest<UserAuthorizationResponse> {
        public string UserName { get; }
        public string Password { get; }

        public UserAuthorizationCommand(
            string userName,
            string password)
        {
            UserName = userName;
            Password = password;
        }
    }
}
