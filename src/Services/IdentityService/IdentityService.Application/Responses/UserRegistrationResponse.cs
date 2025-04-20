namespace IdentityService.Application.Responses
{
    public class UserRegistrationResponse
    {
        public bool IsSuccess { get; }
        public string Message { get; } = string.Empty;

        public UserRegistrationResponse(bool isSuccess, string message)
        {
            IsSuccess = isSuccess;
            Message = message;
        }
    }
}
