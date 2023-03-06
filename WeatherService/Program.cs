using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using WeatherService.Contexts;

var builder = WebApplication.CreateBuilder(args);

var basePath = Path.GetDirectoryName(typeof(Program).Assembly.Location);
var configs = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(Path.Combine(basePath!, "appsettings.json"), false)
                .AddEnvironmentVariables()
                 .Build();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var t = configs.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ForecastContext>(options =>
                options.UseSqlServer(t));

builder.Services.AddCors(policy => {
    policy.AddPolicy("Local", builder =>
        builder.WithOrigins("http://localhost:*", "https://localhost:*", "https://localhost:7278")
        .AllowAnyMethod()
        .AllowAnyHeader());
});

var app = builder.Build();

using (var scope = app.Services.CreateScope()) 
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<ForecastContext>();
        DbInitializer.Initialize(context);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred creating the DB.");
    }
}



app.UseCors("Local");
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
