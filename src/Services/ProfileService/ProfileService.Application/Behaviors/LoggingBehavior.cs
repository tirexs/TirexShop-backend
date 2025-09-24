using MediatR;
using Microsoft.Extensions.Logging;
using ProfileService.Application.Models;

namespace ProfileService.Application.Behaviors;

public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, BaseResponse<TResponse>> where TRequest : notnull
{
    private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

    public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }
    
    public async Task<BaseResponse<TResponse>> Handle(TRequest request, RequestHandlerDelegate<BaseResponse<TResponse>> next, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Началась обработка {RequestName}", typeof(TRequest).Name);
            var response = await next(cancellationToken);
            
            if (response.IsSuccess)
                _logger.LogInformation("Успешная обработка {RequestName}", typeof(TRequest).Name);
            else
                _logger.LogWarning("Обработка {RequestName} завершена с ошибкой: {Error}", 
                    typeof(TRequest).Name, response.Error);
            
            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Неожиданная ошибка при обработке {RequestName}", typeof(TRequest).Name);
            return BaseResponse<TResponse>.Failure($"Неожиданная ошибка: {ex.Message}");
        }
    }
}
