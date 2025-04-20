using Autofac;
using IdentityService.Infrastructure.Services;
using System.Reflection;

namespace IdentityService.API.IoC
{
    public class ApplicationServiceRegistration : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<UserService>()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            builder.RegisterType<JwtTokenService>()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
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
}
