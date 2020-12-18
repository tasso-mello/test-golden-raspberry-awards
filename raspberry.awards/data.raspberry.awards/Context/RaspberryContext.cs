namespace data.raspberry.awards.Context
{
    using data.raspberry.awards.Entities;
    using Microsoft.EntityFrameworkCore;

    public class RaspberryContext : DbContext
    {

        public DbSet<Movie> Movies { get; set; }


        public RaspberryContext(DbContextOptions<RaspberryContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Entities.Movie>().HasKey(c => new { c.Id });
        }
    }
}