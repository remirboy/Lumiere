using Lumiere.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Lumiere.Data
{
    public class LumiereContext : IdentityDbContext<User>
    {
        public LumiereContext(DbContextOptions<LumiereContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<Film> Films { get; set; }
        public DbSet<FilmPoster> Posters { get; set; }
        public DbSet<FilmTrailer> Trailers { get; set; }
        public DbSet<FilmSeance> Seances { get; set; }
        public DbSet<ReservedSeat> ReservedSeats { get; set; }
        public DbSet<FilmFeedback> FilmFeedbacks { get; set; }
    }
}
