using Microsoft.EntityFrameworkCore;
using MusicStorage.Models;

namespace MusicStorage.Data
{
    public class DataContext : DbContext
    {

        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<Genre> genres { get; set; }

        public DbSet<Artist> artists { get; set; }
        public DbSet<Track> tracks { get; set; }
    }
}
