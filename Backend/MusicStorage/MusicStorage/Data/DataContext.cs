using Microsoft.EntityFrameworkCore;
using MusicStorage.Models;

namespace MusicStorage.Data
{
    public class DataContext : DbContext
    {

        public DataContext(DbContextOptions<DataContext> options) : base(options) { }


        public DbSet<Genre> Genres { get; set; }

        public DbSet<Artist> Artists { get; set; }
        public DbSet<Track> Tracks { get; set; }

        public DbSet<TrackArtist> TrackArtists { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TrackArtist>()
                    .HasKey(ta => new { ta.TrackId, ta.ArtistId});
            modelBuilder.Entity<TrackArtist>()
                    .HasOne(t => t.Track)
                    .WithMany(ta => ta.TrackArtists)
                    .HasForeignKey(t => t.TrackId);
            modelBuilder.Entity<TrackArtist>()
                    .HasOne(a => a.Artist)
                    .WithMany(ta => ta.TrackArtists)
                    .HasForeignKey(a => a.ArtistId);
        }
    }
}
