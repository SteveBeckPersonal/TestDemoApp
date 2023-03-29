using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using WeatherService.Contexts;

namespace WeatherService
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            
            services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            var t = Configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<ForecastContext>(options =>
                            options.UseInMemoryDatabase("MyDatabase"));

            services.AddCors(policy => {
                policy.AddPolicy("Local", builder =>
                    builder.WithOrigins("http://localhost:*", "https://localhost:*", "https://localhost:7278")
                    .AllowAnyMethod()
                    .AllowAnyHeader());
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            app.UseSwagger();
            app.UseSwaggerUI();


            app.UseCors("Local");

            app.UseHttpsRedirection();

            app.UseAuthorization();

            using (var scope = app.ApplicationServices.CreateScope())
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

            app.UseRouting();

            app.UseEndpoints(endpoints => 
            {
                endpoints.MapControllers();
            });

        }
    }
}
