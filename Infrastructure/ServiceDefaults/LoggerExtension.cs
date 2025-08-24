using Microsoft.AspNetCore.Builder;
using Serilog;
using Serilog.Formatting.Compact;

namespace ServiceDefaults;

public static class LoggerExtension
{
    public static ConfigureHostBuilder AddLogging(
        this ConfigureHostBuilder host,
        string serviceName,
        string environment)
    {
        host.UseSerilog((ctx, config) =>
        {
            config
                .Enrich.FromLogContext()
                .Enrich.WithProperty("Service", serviceName)
                .Enrich.WithProperty("Environment", environment)
                .WriteTo.Console(new RenderedCompactJsonFormatter()); // <-- JSON формат для stdout
        });
        
        return host;
    }
}