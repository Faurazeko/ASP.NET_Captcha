using Microsoft.EntityFrameworkCore;

using ASP_Captcha.Models;

namespace ASP_Captcha
{
    public class AppDbContext : DbContext
    {
        public DbSet<Captcha> Captchas { get; set; }

        public AppDbContext() => Database.EnsureCreated();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Captcha>().HasAlternateKey(c => c.Id);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=MillionDollarsDb;Trusted_Connection=True;MultipleActiveResultSets=true");
        }
    }
}
