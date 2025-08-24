using Microsoft.Extensions.DependencyInjection;

namespace ServiceDefaults;

public static class CorsExtension
{
    public static IServiceCollection AddCorsSettings(this IServiceCollection services)
    {
        services.AddCors(opt =>
        {
            opt.AddPolicy("AllowAll", builder =>
                builder.WithOrigins(new string[]{
                        "http://localhost:4200",
                        "http://localhost:3000",
                    })
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());
        });

        return services;
    }
}