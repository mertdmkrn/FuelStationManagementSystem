using FuelStationManagementSystem.Middleware;
using FuelStationManagementSystem.Repository;
using FuelStationManagementSystem.Repository.Abstract;
using FuelStationManagementSystem.Repository.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Formatting.Compact;

var builder = WebApplication.CreateBuilder(args);
string currentPath = Path.Combine(AppContext.BaseDirectory.Replace("bin\\Debug\\net8.0\\", ""));


Log.Logger = new LoggerConfiguration()
    .WriteTo.Logger(lc => lc
        .MinimumLevel.Information()
        .WriteTo.File(new CompactJsonFormatter(), "Logs/log.json", rollingInterval: RollingInterval.Day, flushToDiskInterval: TimeSpan.Zero))
    .WriteTo.Logger(lc => lc
        .MinimumLevel.Error()
        .WriteTo.File(new CompactJsonFormatter(), "Logs/error.json", rollingInterval: RollingInterval.Day, flushToDiskInterval: TimeSpan.Zero))
    .CreateLogger();

builder.Host.UseSerilog();

builder.Services.AddCors(p => p.AddPolicy("corsapp", builder =>
{
    builder.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
}));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    var filePath = Path.Combine(currentPath + "FuelStationManagementSystem.xml");
    c.IncludeXmlComments(filePath);
});

builder.Services.AddDbContext<FuelStationDbContext>(options =>
{
    options.UseSqlServer("Data Source=BTA20582\\SQLEXPRESS;Initial Catalog=FuelStationManagementSystemDB;Integrated Security=True;");
});

builder.Services.AddTransient<LoggingMiddleware>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseCors("corsapp");

app.UseMiddleware<LoggingMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
