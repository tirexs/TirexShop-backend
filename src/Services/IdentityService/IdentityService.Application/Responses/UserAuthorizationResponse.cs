namespace IdentityService.Application.Responses
{
    public class UserAuthorizationResponse
    {
        public bool IsSuccess { get; }
        public string Message { get; } = string.Empty;
        public string JwtToken { get; } = string.Empty;

        public UserAuthorizationResponse(bool isSuccess, string message)
        {
            IsSuccess = isSuccess;
            Message = message;
        }

        public UserAuthorizationResponse(bool isSuccess, string message, string jwtToken)
        {
            IsSuccess = isSuccess;
            Message = message;
            JwtToken = jwtToken;
        }
    }
}
