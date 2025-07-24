using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Prometheus;
using Serilog;
using Serilog.Sinks.Grafana.Loki;


var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((ctx, lc) =>
    lc.WriteTo.Console()
        .WriteTo.GrafanaLoki("http://loki:3100")
        .Enrich.FromLogContext()
        .Enrich.WithProperty("Service", "Gateway"));

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
app.UseHttpMetrics();
app.MapMetrics(); 

app.Run();
