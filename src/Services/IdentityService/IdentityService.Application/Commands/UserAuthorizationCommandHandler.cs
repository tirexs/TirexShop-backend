using IdentityService.Application.Responses;
using IdentityService.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace IdentityService.Application.Commands
{
    public class UserAuthorizationCommandHandler : IRequestHandler<UserAuthorizationCommand, UserAuthorizationResponse>
    {
        #region private
        private readonly IUserService _userService;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly ILogger<UserAuthorizationCommandHandler> _logger;
        #endregion

        public UserAuthorizationCommandHandler(
            IUserService userService,
            IJwtTokenService jwtTokenService,
            ILogger<UserAuthorizationCommandHandler> logger)
        {
            _userService = userService;
            _jwtTokenService = jwtTokenService;
            _logger = logger;
        }

        public async Task<UserAuthorizationResponse> Handle(UserAuthorizationCommand command, CancellationToken cancellationToken)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                var user = await _userService.UserAuthorizationAsync(command.UserName, command.Password,
                    cancellationToken);
                var jwtToken = _jwtTokenService.GenerateJwtToken(user);

                _logger.LogInformation($"Пользователь ID: {user.Id} успешно авторизовался");
                return new UserAuthorizationResponse(true, "Пользователь успешно авторизирован", jwtToken);
            }
            catch (OperationCanceledException)
            {
                return new UserAuthorizationResponse(false, "Процесс авторизации прерван");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Не удалось авторизировать пользователя. {ex.Message}");
                return new UserAuthorizationResponse(false, $"Не удалось авторизировать пользователя. {ex.Message}");
            }
        }
    }
}
