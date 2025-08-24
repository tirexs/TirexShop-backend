using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Serilog;
using ServiceDefaults;


var builder = WebApplication.CreateBuilder(args);
var serviceName = "ApiGateway";
var environment = builder.Environment.EnvironmentName;


builder.Host.AddLogging(serviceName, environment);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// ��������� ��������� Ocelot
builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);
builder.Services.AddOcelot();


builder.Services.AddCorsSettings();
builder.Services.AddTelemetry(serviceName);


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
