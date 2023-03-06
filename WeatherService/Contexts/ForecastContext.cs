using Microsoft.EntityFrameworkCore;

namespace WeatherService.Contexts
{
    public class ForecastContext :DbContext
    {
        public ForecastContext(DbContextOptions<ForecastContext> options) : base(options)
        {

        }

        public DbSet<WeatherForecast> Forecasts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<WeatherForecast>().ToTable("Forecasts");
        }
    }
}
