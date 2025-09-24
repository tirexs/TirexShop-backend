using Microsoft.Extensions.Logging;
using ProfileService.Application.Models;

namespace ProfileService.Application.Handlers;

public abstract class BaseHandler<THandler>
{
    #region private
    private ILogger<THandler> _logger;
    #endregion

    protected BaseHandler(ILogger<THandler> logger)
    {
        _logger = logger;
    }

    protected BaseResponse<T> GetSuccessResponse<T>(T result, string? message = null)
    {
        if (message is not null)
        {
            _logger.LogInformation(message);
        }
        
        return BaseResponse<T>.Success(result);
    }
    
    protected BaseResponse<T> GetFailureResponse<T>(string message)
    {
        _logger.LogInformation(message);
        return BaseResponse<T>.Failure(message);
    }
}