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
        public DbSet<Fall2024_Assignment3_npmckivergan.Models.Actor> Actor { get; set; } = default!;
        public DbSet<Fall2024_Assignment3_npmckivergan.Models.Movie> Movie { get; set; } = default!;
        public DbSet<Fall2024_Assignment3_npmckivergan.Models.ActorMovie> ActorMovie { get; set; } = default!;
    }
}
