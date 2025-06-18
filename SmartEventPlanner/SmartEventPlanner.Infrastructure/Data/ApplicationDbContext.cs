using Microsoft.EntityFrameworkCore;
using SmartEventPlanner.Domain.Entities;

namespace SmartEventPlanner.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Event> Events { get; set; }
        public DbSet<WeatherData> WeatherData { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Event>()
                .HasOne(e => e.WeatherData)
                .WithOne()
                .HasForeignKey<Event>("WeatherDataId")
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}