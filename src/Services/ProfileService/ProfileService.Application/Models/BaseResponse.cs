namespace ProfileService.Application.Models;

public class BaseResponse<T>
{
    public bool IsSuccess { get; }
    public T Value { get; }
    public string? Error { get; }
    
    private BaseResponse(bool isSuccess, T value, string? error)
    {
        IsSuccess = isSuccess;
        Value = value;
        Error = error;
    }
    
    public static BaseResponse<T> Success(T value) => new BaseResponse<T>(true, value, null);
    public static BaseResponse<T?> Failure(string? error) => new BaseResponse<T?>(false, default, error);
}