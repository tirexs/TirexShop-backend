
using System.Reflection;
using Autofac;
using Microsoft.Extensions.Hosting;
using ProfileService.Domain.Interfaces.Repositories;
using ProfileService.Infrastructure.Messaging;
using ProfileService.Infrastructure.Persistence;

namespace ProfileService.Infrastructure.Ioc;

public class ApplicationServiceRegistration : Autofac.Module
{
    protected override void Load(ContainerBuilder builder)
    {
        
        // Репозитории
        builder.RegisterType<ProfileRepository>()
            .As<IProfileRepository>()
            .InstancePerLifetimeScope();

        // Kafka Consumer как HostedService
        builder.RegisterType<KafkaKeycloakConsumer>()
            .As<IHostedService>()
            .SingleInstance();
    }

    private Func<ParameterInfo, IComponentContext, bool> Named(string name)
    {
        return (p, c) => p.Name == name;
    }

    private Func<ParameterInfo, IComponentContext, object> InjectWith<T>(string name)
    {
        return (p, c) => c.ResolveNamed<T>(name);
    }
}