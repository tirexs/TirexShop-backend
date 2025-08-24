using Autofac;
using Autofac.Extensions.DependencyInjection;
using IdentityService.API.Extensions;
using IdentityService.API.IoC;
using Serilog;
using ServiceDefaults;

var builder = WebApplication.CreateBuilder(args);
var serviceName = "IdentityService";
var environment = builder.Environment.EnvironmentName;


builder.Host.AddLogging(serviceName, environment);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


//Autofac
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureContainer<ContainerBuilder>(builder =>
                {
                    builder.RegisterModule(new ApplicationServiceRegistration());
                });

builder.Services.AddApplication()
    .AddInfrastructure(builder.Configuration);

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
