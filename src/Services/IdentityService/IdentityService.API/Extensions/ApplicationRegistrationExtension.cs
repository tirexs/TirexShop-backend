using IdentityService.Application.Commands;
using System.Reflection;

namespace IdentityService.API.Extensions
{
    public static class ApplicationRegistrationExtension
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            // 1. Регистрация MediatR (CQRS)
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(typeof(UserRegistrationCommandHandler).Assembly);
                cfg.Lifetime = ServiceLifetime.Scoped;
            });



            return services;
        }
    }
}
