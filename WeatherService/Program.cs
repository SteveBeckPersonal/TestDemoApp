using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using WeatherService;
using WeatherService.Contexts;

var builder = WebApplication.CreateBuilder(args);

var startup = new Startup(builder.Configuration);
startup.ConfigureServices(builder.Services);

var app = builder.Build();
startup.Configure(app, builder.Environment);
app.Run();

