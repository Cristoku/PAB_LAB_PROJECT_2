using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WeatherAPI.Domain;

namespace WeatherAPI.Persistence
{
    public class ApplicationDbContext(DbContextOptions contextOptions) : IdentityDbContext(contextOptions)
    {
        public DbSet<Weather> Weathers { get; set; }

        public DbSet<Region> Regions { get; set; }

        public DbSet<Country> Countries { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Weather>(entity => entity.HasOne(r => r.Region));

            modelBuilder.Entity<Region>(entity => entity.HasOne(c => c.Country).WithMany(r => r.Regions));

            base.OnModelCreating(modelBuilder);
        }
    }
}