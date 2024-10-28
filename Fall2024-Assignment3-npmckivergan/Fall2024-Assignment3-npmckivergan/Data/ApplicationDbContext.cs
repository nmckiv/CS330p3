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
    }
}
