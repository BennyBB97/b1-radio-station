using Microsoft.EntityFrameworkCore;
using MusicStorage.Data;
using MusicStorage.Dto;
using MusicStorage.Interfaces;
using MusicStorage.Models;
using System.Xml.Linq;

namespace MusicStorage.Repository
{
    public class MusicRepository : IMusicRepository
    {
        private readonly DataContext context;

        public MusicRepository(DataContext context) 
        {
            this.context = context;
        }

        public void createTrack(Track track)
        {
            throw new NotImplementedException();
        }

        public ICollection<Genre> GetGenres()
        {
            return context.Genres.OrderBy(p => p.GenreId).ToList();
        }

        public ICollection<Track> GetTracks()
        {
            throw new NotImplementedException();
        }

        public ICollection<Track> SearchTracks(SearchTracksDto searchTracks)
        {
            searchTracks.SearchTerm = "Taylor Swift";
            var query = from track in context.Tracks
                        join trackArtist in context.TrackArtists on track.TrackId equals trackArtist.TrackId
                        join artist in context.Artists on trackArtist.ArtistId equals artist.ArtistId  
                        join genre in context.Genres on track.Genre.GenreId equals genre.GenreId
                        where track.Titel.ToLower().Equals(searchTracks.SearchTerm.ToLower()) || 
                        artist.Name.ToLower().Equals(searchTracks.SearchTerm.ToLower()) ||
                        genre.Name.ToLower().Equals(searchTracks.SearchTerm.ToLower())                        
                        select track;

            return query.ToList();
        }

        public void updateTrack(Track track)
        {
            throw new NotImplementedException();
        }
    }
}
