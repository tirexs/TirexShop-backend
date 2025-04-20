using IdentityService.Application.Responses;
using IdentityService.Domain.Interfaces;
using MediatR;

namespace IdentityService.Application.Commands
{
    public class UserRegistrationCommandHandler : IRequestHandler<UserRegistrationCommand, UserRegistrationResponse>
    {
        #region private
        private readonly IUserService _userService;
        #endregion

        public UserRegistrationCommandHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<UserRegistrationResponse> Handle(UserRegistrationCommand command, CancellationToken cancellationToken)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                await _userService.UserRegistrationAsync(command.UserName, command.Email, command.Password, cancellationToken);

                return new UserRegistrationResponse(true, "Пользователь успешно зарегистрирован");
            }
            catch (OperationCanceledException) { return new UserRegistrationResponse(false, "Процесс регистрации прерван"); }
            catch (Exception ex) { return new UserRegistrationResponse(false, $"Не удалось зарегистрировать пользователя. {ex.Message}"); }
        }
    }
}
