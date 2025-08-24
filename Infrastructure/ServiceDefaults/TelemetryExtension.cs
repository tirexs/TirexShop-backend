using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Exporter;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace ServiceDefaults;

public static class TelemetryExtension
{
    public static IServiceCollection AddTelemetry(
        this IServiceCollection services,
        string serviceName)
    {
        
        // OpenTelemetry Resource (used by all signals: metrics, tracing)
        var resourceBuilder = ResourceBuilder.CreateDefault()
            .AddService(serviceName)
            .AddEnvironmentVariableDetector();

        // Add OpenTelemetry
        services.AddOpenTelemetry()
            .ConfigureResource(b => b.AddService(serviceName))
            .WithMetrics(metrics =>
            {
                metrics
                    .SetResourceBuilder(resourceBuilder)
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddRuntimeInstrumentation()
                    .AddProcessInstrumentation()
                    .AddOtlpExporter(o =>
                    {
                        o.Endpoint = new Uri("http://alloy:4317"); // или alloy:4318 для HTTP
                        o.Protocol = OtlpExportProtocol.Grpc;
                    });
            })
            .WithTracing(tracing =>
            {
                tracing
                    .SetResourceBuilder(resourceBuilder)
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddOtlpExporter(o =>
                    {
                        o.Endpoint = new Uri("http://alloy:4317"); // или alloy:4318 для HTTP
                        o.Protocol = OtlpExportProtocol.Grpc;
                    });
            });
        
        return services;
    }
}