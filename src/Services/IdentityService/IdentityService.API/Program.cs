using Autofac;
using Autofac.Extensions.DependencyInjection;
using IdentityService.API.Extensions;
using IdentityService.API.IoC;
using Prometheus;
using Serilog;
using Serilog.Formatting.Compact;
using Serilog.Sinks.Grafana.Loki;

var builder = WebApplication.CreateBuilder(args);


builder.Host.UseSerilog((ctx, config) =>
{
    config
        .Enrich.FromLogContext()
        .Enrich.WithProperty("Service", "IdentityService")
        .Enrich.WithProperty("Environment", ctx.HostingEnvironment.EnvironmentName)
        .WriteTo.Console(new RenderedCompactJsonFormatter()); // <-- JSON формат для stdout
});

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
app.UseHttpMetrics();
app.MapMetrics();

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
