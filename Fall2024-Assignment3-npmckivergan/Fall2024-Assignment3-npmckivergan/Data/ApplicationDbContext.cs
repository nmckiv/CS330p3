using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Fall2024_Assignment3_npmckivergan.Models;

namespace Fall2024_Assignment3_npmckivergan.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Actor> Actor { get; set; } = default!;
        public DbSet<Movie> Movie { get; set; } = default!;
        public DbSet<Review> Review { get; set; } = default!;
        public DbSet<ActorMovie> ActorMovie { get; set; } = default!;

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    base.OnModelCreating(modelBuilder);

        //    // Define the one-to-many relationship between Movie and Review
        //    modelBuilder.Entity<Review>()
        //        .HasOne(r => r.Movie) // Each Review has one Movie
        //        .WithMany(m => m.Reviews) // Each Movie has many Reviews
        //        .HasForeignKey(r => r.MovieId); // Foreign key in Review
        //}
    }
}
