using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProfileService.Infrastructure.Persistence;
using ProfileService.Infrastructure.Ioc;
using Microsoft.Extensions.Hosting;

namespace ProfileService.Infrastructure.Extensions;

public static class InfrastructureExtensions
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration,
        IHostBuilder  host)
    {
        // Регистрация DbContext
        services.AddDbContext<ProfileDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));
        
        
        // Autofac
         host.UseServiceProviderFactory(new AutofacServiceProviderFactory())
             .ConfigureContainer<ContainerBuilder>((context, builder) =>
             {
                 builder.RegisterModule(new ApplicationServiceRegistration());
             });
        
        return services;
    }
    
}