using IdentityService.Application.Responses;
using IdentityService.Domain.Interfaces;
using MediatR;

namespace IdentityService.Application.Commands
{
    public class UserAuthorizationCommandHandler : IRequestHandler<UserAuthorizationCommand, UserAuthorizationResponse>
    {
        #region private
        private readonly IUserService _userService;
        private readonly IJwtTokenService _jwtTokenService;
        #endregion

        public UserAuthorizationCommandHandler(
            IUserService userService,
            IJwtTokenService jwtTokenService)
        {
            _userService = userService;
            _jwtTokenService = jwtTokenService;
        }

        public async Task<UserAuthorizationResponse> Handle(UserAuthorizationCommand command, CancellationToken cancellationToken)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                var user = await _userService.UserAuthorizationAsync(command.UserName, command.Password, cancellationToken);
                var jwtToken = _jwtTokenService.GenerateJwtToken(user);

                return new UserAuthorizationResponse(true, "Пользователь успешно авторизирован", jwtToken);
            }
            catch (OperationCanceledException) { return new UserAuthorizationResponse(false, "Процесс авторизации прерван"); }
            catch (Exception ex) { return new UserAuthorizationResponse(false, $"Не удалось авторизировать пользователя. {ex.Message}"); }
        }
    }
}
