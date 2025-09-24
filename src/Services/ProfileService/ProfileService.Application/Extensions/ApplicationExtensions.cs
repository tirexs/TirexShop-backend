using MediatR;
using Microsoft.Extensions.DependencyInjection;
using ProfileService.Application.Behaviors;
using ProfileService.Application.Commands;

namespace ProfileService.Application.Extensions;

public static class ApplicationExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(CreateProfileCommand).Assembly);
            cfg.Lifetime = ServiceLifetime.Scoped;
        });
        
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));

        return services;
    }
}