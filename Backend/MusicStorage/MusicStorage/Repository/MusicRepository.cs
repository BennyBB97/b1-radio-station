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

        public bool CheckArtistExist(string artistName)
        {
            return context.Artists.Any(a => a.Name.Equals(artistName));
        }

        public bool CheckTrackExist(string trackTitel)
        {
            return context.Tracks.Any(t => t.Titel.Equals(trackTitel));
        }

        public bool CheckTrackExist(int trackId)
        {
            return context.Tracks.Any(t => t.TrackId == trackId);
        }

        public bool CheckGenreExist(int genreId)
        {
            return context.Genres.Any(g => g.GenreId == genreId);
        }

        public bool CheckTrackHasUniqueArtists(List<string> artistNamens, string trackTitel)
        {
            bool result = true;
            bool caseMoreArtists = false;
            var query = from track in context.Tracks
                        join trackArtist in context.TrackArtists on track.TrackId equals trackArtist.TrackId
                        join artist in context.Artists on trackArtist.ArtistId equals artist.ArtistId
                        where track.Titel.ToLower().Equals(trackTitel.ToLower())
                        select new { trackId = track.TrackId, artistName = artist.Name };

            foreach (var tracks in query.ToList().GroupBy(t => t.trackId))
            {
                List<string> artistNamensList = tracks.Select(te => te.artistName).ToList();
                var intersected = artistNamensList.Intersect(artistNamens, StringComparer.OrdinalIgnoreCase);

                //Sonderfall, wenn schnittmenge mehr als ein artist hat
                if (intersected.Count() == artistNamensList.Count())
                    result = false; //todo break? 

                if (!(intersected.Count() < artistNamens.Count()))
                {
                    
                }                                   
            }
            return result;
        }


        public void CreateTrack(CreateTrackDto createTrack)
        {
            List<Artist> artistList = [];
            List<Artist> artistToAddList = [];
            List<TrackArtist> trackArtistList = [];

            var genre = context.Genres.Where(g => g.GenreId == createTrack.GenreId).FirstOrDefault();

            Track track = new Track()
            {                
                Titel = createTrack.Titel,
                Genre = genre, //todo
            };
            context.Tracks.Add(track);

            foreach (var name in createTrack.ArtistNames)
            {
                if (!CheckArtistExist(name))
                {
                    Artist artist = new Artist();
                    {
                        artist.Name = name;
                    }
                    artistList.Add(artist);
                    context.Artists.Add(artist);
                }
                else
                {
                    var artist = context.Artists.FirstOrDefault(a => a.Name == name);
                    if (artist is not null)
                    artistList.Add(artist);
                }                
            }

            foreach (var artist in artistList) 
            {                
                TrackArtist trackArtist = new TrackArtist()
                {
                    Track = track,
                    Artist = artist,
                };
                trackArtistList.Add(trackArtist);
            }            
            //context.Artists.AddRange(artistList);
            context.TrackArtists.AddRange(trackArtistList);
            context.SaveChanges();            
        }

        public ICollection<Genre> GetGenres()
        {
            return context.Genres.OrderBy(p => p.GenreId).ToList();
        }

        public List<TrackDto> GetAllTracks()
        {
            List<TrackDto> trackList = [];
            var query = from track in context.Tracks
                        join trackArtist in context.TrackArtists on track.TrackId equals trackArtist.TrackId
                        join artist in context.Artists on trackArtist.ArtistId equals artist.ArtistId
                        join genre in context.Genres on track.Genre.GenreId equals genre.GenreId
                        select new { track.TrackId, track.Titel, artist.ArtistId, artist.Name , genre.GenreId, genreName = genre.Name };

            foreach (var item in query)
            {                
                var trackExistIndex = trackList.FindIndex(i => i.TrackId == item.TrackId);                
                if (trackExistIndex < 0)
                {
                    TrackDto track = new TrackDto();
                    track.TrackId = item.TrackId;
                    track.Titel = item.Titel;

                    GenreDto genre = new GenreDto();
                    genre.GenreId = item.GenreId;
                    genre.Name = item.genreName;
                    track.Genre = genre;

                    List<ArtistDto> artistList = [];
                    ArtistDto artist = new ArtistDto();
                    artist.ArtistId = item.ArtistId;
                    artist.Name = item.Name;
                    artistList.Add(artist);
                    track.Artists = artistList;

                    trackList.Add(track);
                }
                else
                {
                    ArtistDto artist = new ArtistDto();
                    artist.ArtistId = item.ArtistId;
                    artist.Name = item.Name;
                    trackList[trackExistIndex].Artists.Add(artist);
                }              
            }
            return trackList;
        }

        public ICollection<TrackDto> SearchTracks(SearchTracksDto searchTracks)
        {
            searchTracks.SearchTerm = "Taylor Swift";
            var query = from track in context.Tracks
                        join trackArtist in context.TrackArtists on track.TrackId equals trackArtist.TrackId
                        join artist in context.Artists on trackArtist.ArtistId equals artist.ArtistId  
                        join genre in context.Genres on track.Genre.GenreId equals genre.GenreId
                        where track.Titel.ToLower().Equals(searchTracks.SearchTerm.ToLower()) || 
                        artist.Name.ToLower().Equals(searchTracks.SearchTerm.ToLower()) ||
                        genre.Name.ToLower().Equals(searchTracks.SearchTerm.ToLower())                        
                        select new { track.Titel, track.Genre, artist.ArtistId, artist.Name };

            return GetAllTracks();           
        }

        public void UpdateTrack(UpdateTrackDto trackDto)
        {
            var track = context.Tracks.Where(t => t.TrackId == trackDto.TrackId).FirstOrDefault();
            var genre = context.Genres.Where(g => g.GenreId == trackDto.GenreId).FirstOrDefault();

            track.Titel = trackDto.Titel;
            track.Genre = genre;

            context.Tracks.Update(track);
            context.SaveChanges();


            /*
            var artist = context.Artists.Where(a => a.ArtistId == trackDto.Artists[0].ArtistId).FirstOrDefault();


            //var trackArtist = context.TrackArtists.Where(t => t.TrackId == track.TrackId && t.ArtistId == artist.ArtistId).FirstOrDefault();
            var trackArtist = context.TrackArtists.Where(t => t.TrackId == track.TrackId).ToList();

            List<TrackArtist> trackArtists = new List<TrackArtist>();
            if (trackArtist != null)
            {
                Artist artist1 = new Artist()
                {
                    Name = "neuerArtist"
                };

                TrackArtist trackArtist1 = new TrackArtist()
                {
                    Track = track,
                    Artist = artist1
                    
                };
                
                trackArtists.Add(trackArtist1);
            }

            List<Artist> artists = new List<Artist>();
            artists.Add(artist);

           

            if (track != null) 
                track.Titel = trackDto.Titel;

            track.Genre = genre;

            track.TrackArtists = trackArtists;
            //track.Artists = artists;

            
            //var genre = context.Genres.Where(g => g.GenreId == createTrack.GenreId).FirstOrDefault();
            */
            
        }

        public void Delete(int trackId)
        {
            //todo auslagern
            var track = context.Tracks.Where(t => t.TrackId == trackId).FirstOrDefault();
            context.Tracks.Remove(track);
        }

        public void DeleteTrack(int trackId)
        {
            //todo auslagern
            var track = context.Tracks.Where(t => t.TrackId == trackId).FirstOrDefault();
            context.Tracks.Remove(track);
            context.SaveChanges();
        }

        
    }
}
