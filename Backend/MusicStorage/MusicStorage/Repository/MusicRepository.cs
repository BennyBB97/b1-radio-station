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

        /// <summary>
        /// Checks if an artist exists by name
        /// </summary>
        /// <param name="artistName">Name of the artist</param>
        /// <returns></returns>
        public bool CheckArtistExist(string artistName)
        {
            return context.Artists.Any(a => a.Name.ToLower().Equals(artistName.ToLower()));
        }

        /// <summary>
        /// Checks if an track exists by titel
        /// </summary>
        /// <param name="trackTitel">Name of the titel</param>
        /// <returns></returns>
        public bool CheckTrackExist(string trackTitel)
        {
            return context.Tracks.Any(t => t.Titel.Equals(trackTitel));
        }

        /// <summary>
        /// Checks if an track exists by track id
        /// </summary>
        /// <param name="trackTitel">Id of the track</param>
        /// <returns></returns>
        public bool CheckTrackExist(int trackId)
        {
            return context.Tracks.Any(t => t.TrackId == trackId);
        }

        /// <summary>
        /// Checks if the genre exist
        /// </summary>
        /// <param name="genreId">Id of the genre</param>
        /// <returns></returns>
        public bool CheckGenreExist(int genreId)
        {
            return context.Genres.Any(g => g.GenreId == genreId);
        }

        /// <summary>
        /// Checks whether the track is unique after updating the title.
        /// A title is unique if the combination of title and artists is unique
        /// </summary>
        /// <param name="trackId"></param>
        /// <param name="newTrackTitel"></param>
        /// <returns>True, if the track is unique</returns>
        public bool CheckTrackHasUniquieArtistAfterUpdate(int trackId, string newTrackTitel)
        {
            bool result = true;
            List<string> orgArtists = [];

            var tracksWithSameNamens = from track in context.Tracks
                        join trackArtist in context.TrackArtists on track.TrackId equals trackArtist.TrackId
                        join artist in context.Artists on trackArtist.ArtistId equals artist.ArtistId
                        where track.Titel.ToLower().Equals(newTrackTitel.ToLower())
                        select new { trackId = track.TrackId, artistName = artist.Name };

            var orgTrack = from track in context.Tracks
                        join trackArtist in context.TrackArtists on track.TrackId equals trackArtist.TrackId
                        join artist in context.Artists on trackArtist.ArtistId equals artist.ArtistId
                        where track.TrackId == trackId
                        select  artist;

            orgArtists = orgTrack.Select(a => a.Name).ToList();

            foreach (var tracks in tracksWithSameNamens.ToList().GroupBy(t => t.trackId))
            {
                List<string> artistNamensList = tracks.Select(te => te.artistName).ToList();                
                var intersected = artistNamensList.Intersect(orgArtists, StringComparer.OrdinalIgnoreCase);

                //Special case: Check whether the intersection has more or less artists than the track
                if ((intersected.Count() == orgArtists.Count()))
                {
                    if (intersected.Count() == artistNamensList.Count())
                        result = false; //There is a track that has the same artists
                }
            }
            return result;
        }

        /// <summary>
        /// Checks whether the track titel has unique artists.
        /// A title is unique if the combination of title and artist is unique
        /// </summary>
        /// <param name="artistNamens">Artists to be tested</param>
        /// <param name="trackTitel">The titel of the track to be searched</param>
        /// <returns></returns>
        public bool CheckTrackHasUniqueArtists(List<string> artistNamens, string trackTitel)
        {
            bool result = true;
            var query = from track in context.Tracks
                        join trackArtist in context.TrackArtists on track.TrackId equals trackArtist.TrackId
                        join artist in context.Artists on trackArtist.ArtistId equals artist.ArtistId
                        where track.Titel.ToLower().Equals(trackTitel.ToLower())
                        select new { trackId = track.TrackId, artistName = artist.Name };

            foreach (var tracks in query.ToList().GroupBy(t => t.trackId))
            {
                List<string> artistNamensList = tracks.Select(te => te.artistName).ToList();
                var intersected = artistNamensList.Intersect(artistNamens, StringComparer.OrdinalIgnoreCase);

                //Special case: Check whether the intersection has more or less artists than the track
                if ((intersected.Count() == artistNamens.Count()))
                {
                    if (intersected.Count() == artistNamensList.Count())
                        result = false; //There is a track that has the same artists
                }                                
            }
            return result;
        }

        /// <summary>
        /// Returns all artists from a specific track
        /// </summary>
        /// <param name="trackId"></param>
        /// <returns></returns>
        public List<ArtistDto> GetAllArtistsFromTrack(int trackId)
        {
            List <ArtistDto> artists= [];
            var query = from artist in context.Artists
                        join trackArtist in context.TrackArtists on artist.ArtistId equals trackArtist.ArtistId
                        where trackArtist.TrackId == trackId
                        select artist;
            foreach (var artist in query)
            {
                ArtistDto artistDto = new ArtistDto()
                {
                    ArtistId = artist.ArtistId,
                    Name = artist.Name
                };
                artists.Add(artistDto);
            }
            return artists;
        }

        /// <summary>
        /// Creates a new Track
        /// </summary>
        /// <param name="createTrack">A track object with all the necessary information</param>
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
                    var artist = getArtist(name);
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
            context.TrackArtists.AddRange(trackArtistList);
            context.SaveChanges();            
        }

        /// <summary>
        /// Search for all existing genres and returns them
        /// </summary>
        /// <returns>Returns all existing genres</returns>
        public List<GenreDto> GetGenres()
        {
            var genres = context.Genres.OrderBy(p => p.GenreId).ToList();
            List<GenreDto> genreList = [];
            foreach (var genre in genres)
            {
                GenreDto genreDto = new GenreDto()
                {
                    GenreId = genre.GenreId,
                    Name = genre.Name,
                };
                genreList.Add(genreDto);
            }
            return genreList;
        }

        /// <summary>
        /// Searches for all existing tracks and returns them
        /// </summary>
        /// <returns>Returns all tracks with the respective artists and genre</returns>
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

        /// <summary>
        /// Searches for a track using a search term
        /// If no search type is specified, the search term is searched everywhere
        /// </summary>
        /// <param name="searchTerm">Term to be searched</param>
        /// <returns>List of Tracks with artists and genre type</returns>
        public List<TrackDto> SearchTracks(string searchTerm)
        {
            List<TrackDto> trackList = [];
            //todo - Differentiate between search type            
            var query = from track in context.Tracks
                        join trackArtist in context.TrackArtists on track.TrackId equals trackArtist.TrackId
                        join artist in context.Artists on trackArtist.ArtistId equals artist.ArtistId  
                        join genre in context.Genres on track.Genre.GenreId equals genre.GenreId
                        where track.Titel.ToLower().Equals(searchTerm.ToLower()) || 
                        artist.Name.ToLower().Equals(searchTerm.ToLower()) ||
                        genre.Name.ToLower().Equals(searchTerm.ToLower())
                        select new { track.TrackId, track.Titel, artist.ArtistId, artist.Name, genre.GenreId, genreName = genre.Name };

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

        /// <summary>
        /// Updates a track. The track title and the genre are updated
        /// </summary>
        /// <param name="trackDto">Track object with all necessary information to update</param>
        public void UpdateTrack(UpdateTrackDto trackDto)
        {
            var track = getTrack(trackDto.TrackId);
            var genre = context.Genres.Where(g => g.GenreId == trackDto.GenreId).FirstOrDefault(); //todo

            track.Titel = trackDto.Titel;
            track.Genre = genre;

            context.Tracks.Update(track);
            context.SaveChanges();
        }

        /// <summary>
        /// Deletes a track based on the id
        /// </summary>
        /// <param name="trackId">Track id to be deleted</param>
        public void DeleteTrack(int trackId)
        {
            var track = getTrack(trackId);
            context.Tracks.Remove(track);
            context.SaveChanges();
        }

        /// <summary>
        /// Check if the artist id exists
        /// </summary>
        /// <param name="artistId"></param>
        /// <returns>Artist id to be checked</returns>
        public bool CheckArtistExist(int artistId)
        {
            return context.Artists.Any(a => a.ArtistId == artistId);
        }

        /// <summary>
        /// Check whether the artist is already assigned to the track
        /// </summary>
        /// <param name="trackId">Track to be checked</param>
        /// <param name="artistId">Artist to be checked</param>
        /// <returns></returns>
        public bool CheckArtistBelongsToTrack(int trackId, int artistId)
        {
            var query = from trackArtist in context.TrackArtists
                        where trackArtist.TrackId == trackId && trackArtist.ArtistId == artistId
                        select trackArtist;                
            return query.Any();           
        }

        /// <summary>
        /// todo
        /// </summary>
        /// <param name="trackId"></param>
        /// <returns></returns>
        public bool CheckTrackHasAnArtistAfterDelete(int trackId)
        {
            //todo
            bool result = false;
            var query = from trackArtist in context.TrackArtists
                        where trackArtist.TrackId == trackId
                        select trackArtist;

            if (query.ToList().Count > 1)
                result = true;
            return result;
        }

        /// <summary>
        /// Deletes artist from a track
        /// </summary>
        /// <param name="trackId">Track from which the artist is to be deleted</param>
        /// <param name="artistId">Artist to delete</param>
        public void DeleteArtistFromTrack(int trackId, int artistId)
        {
            var query = (from trackArtist in context.TrackArtists
                        where trackArtist.TrackId == trackId && trackArtist.ArtistId == artistId
                        select trackArtist).SingleOrDefault();

            if (query is not null)
                context.TrackArtists.Remove(query); //todo
                context.SaveChanges();
        }

        /// <summary>
        /// Add an artist to a track
        /// </summary>
        /// <param name="trackId">Track in which the artist is to be added</param>
        /// <param name="artist">Artist object to be added</param>
        public void AddArtistToTrack(int trackId, ArtistDto artist)
        {
            TrackArtist trackArtist = new TrackArtist();
            var track = context.Tracks.Where(t => t.TrackId == trackId).FirstOrDefault();

            if (!CheckArtistExist(artist.Name))
            {
                Artist newArtist = new Artist()
                {
                   Name = artist.Name
                };
                trackArtist.Artist = newArtist;
                context.Artists.Add(newArtist);
            }
            else
            {
                var oldArtist = getArtist(artist.Name);
                if (artist is not null)
                    trackArtist.Artist = oldArtist;
            }
            trackArtist.TrackId = trackId;
            context.TrackArtists.Add(trackArtist);
            context.SaveChanges();
        }

        /// <summary>
        /// Search track based on the id
        /// </summary>
        /// <param name="trackId">Track to be searched</param>
        /// <returns>A Track</returns>
        public Track getTrack(int trackId)
        {
            return context.Tracks.Where(t => t.TrackId == trackId).FirstOrDefault();
        }

        /// <summary>
        /// Search artist based on the name
        /// </summary>
        /// <param name="artistName">Artist name to be searched</param>
        /// <returns>The searched artist</returns>
        public Artist getArtist(string artistName)
        {
            return context.Artists.Where(a => a.Name.ToLower().Equals(artistName.ToLower())).FirstOrDefault();
        }
    }
}
