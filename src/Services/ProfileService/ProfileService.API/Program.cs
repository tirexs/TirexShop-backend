using ProfileService.Application.Extensions;
using ProfileService.Infrastructure.Extensions;
using Serilog;
using ServiceDefaults;

var builder = WebApplication.CreateBuilder(args);
var serviceName = "ProfileService";
var environment = builder.Environment.EnvironmentName;


builder.Host.AddLogging(serviceName, environment); // настройка логирования

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer()
    .AddSwaggerGen() // добавления Swagger
    .AddApplication() // регистрация слоя Application
    .AddInfrastructure(builder.Configuration, builder.Host) // регистрация слоя Infrastructure
    .AddCorsSettings() // настройка cors политики
    .AddTelemetry(serviceName); // настройка метрик и трейсов

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");
app.UseAuthorization();
app.UseAuthentication();
app.MapControllers();

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