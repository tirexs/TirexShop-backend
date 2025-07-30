using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using OpenTelemetry.Exporter;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Serilog;
using Serilog.Formatting.Compact;


var builder = WebApplication.CreateBuilder(args);
var serviceName = "ApiGateway";
var environment = builder.Environment.EnvironmentName;

builder.Host.UseSerilog((ctx, config) =>
{
    config
        .Enrich.FromLogContext()
        .Enrich.WithProperty("Service", serviceName)
        .Enrich.WithProperty("Environment", environment)
        .WriteTo.Console(new RenderedCompactJsonFormatter()); // <-- JSON формат для stdout
});

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// ��������� ��������� Ocelot
builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);
builder.Services.AddOcelot();

builder.Services.AddCors(opt =>
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

// OpenTelemetry Resource (used by all signals: metrics, tracing)
var resourceBuilder = ResourceBuilder.CreateDefault()
    .AddService(serviceName)
    .AddEnvironmentVariableDetector();

// Add OpenTelemetry
builder.Services.AddOpenTelemetry()
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


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");
app.UseAuthorization();
app.MapControllers();

// ��������� middleware Ocelot
await app.UseOcelot();

try
{
    Log.Information("Starting up");
    app.Run();   
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application start-up failed");
}
finally
{
    Log.CloseAndFlush();
}
