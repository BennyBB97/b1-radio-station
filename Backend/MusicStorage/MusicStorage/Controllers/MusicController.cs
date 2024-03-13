using Microsoft.AspNetCore.Mvc;
using MusicStorage.Dto;
using MusicStorage.Interfaces;
using MusicStorage.Models;

namespace MusicStorage.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MusicController : Controller
    {

        private readonly IMusicRepository musicRepository;

        public MusicController(IMusicRepository musicRepository)
        {
            this.musicRepository = musicRepository;
        }

        [HttpGet("genres")]
        public IActionResult GetGenres()
        {
            var genres = musicRepository.GetGenres();
            return Ok(genres);
        }

        [HttpGet("tracks")]
        public IActionResult GetAllTracks()
        {
            var tracks = musicRepository.GetAllTracks();            
            return Ok(tracks);
        }

        [HttpGet("track/{searchTerm}/{type}")]
        public IActionResult SearchTrack(string searchTerm, string type)
        {
            //todo - Implement search type
            if (searchTerm.Equals(""))
                return BadRequest("Please define a search term");

            var tracks = musicRepository.SearchTracks(searchTerm);
            return Ok(tracks);
        }

        [HttpPost("track")]
        public IActionResult CreateTrack([FromBody] CreateTrackDto track)
        {
            if (track.ArtistNames.Count == 0)
                return BadRequest("No artist was specified");

            if(track.Titel.Equals("") || track.Titel.Length >= 30)
                return BadRequest("The titel must be between 1 and 30 characters long.");

            if(!musicRepository.CheckGenreExist(track.GenreId))
                return NotFound("The genre does not exist");

            if (musicRepository.CheckTrackExist(track.Titel))
            {
                if (!musicRepository.CheckTrackHasUniqueArtists(track.ArtistNames, track.Titel))
                {
                    return BadRequest("Track titel already exist and musst have unique artists");
                }
            }

            musicRepository.CreateTrack(track);
            return Created();
        }

        [HttpPut("track")]
        public IActionResult UpdateTrack([FromBody] UpdateTrackDto track)
        {
            if (track.Titel.Equals("") || track.Titel.Length >= 30)
                return BadRequest("The titel must be between 1 and 30 characters long.");

            if (!musicRepository.CheckTrackExist(track.TrackId))
                return NotFound("Track does not exist");

            if (!musicRepository.CheckGenreExist(track.GenreId))
                return NotFound("Genre does not exist");

            if (!musicRepository.CheckTrackHasUniquieArtistAfterUpdate(track.TrackId, track.Titel))
            {
                return BadRequest("Track titel already exist and musst have unique artists");
            }
            musicRepository.UpdateTrack(track);
            return Ok();
        }

        [HttpPut("track/artist")]
        public IActionResult AddArtistToTrack([FromBody] AddArtist trackArtist)
        {
            if (trackArtist.Name.Equals("") || trackArtist.Name.Length >= 20)
                return BadRequest("The artist must be between 1 and 20 characters long.");

            if (!musicRepository.CheckTrackExist(trackArtist.TrackId))
                return NotFound("Track does not exist");

            if (musicRepository.CheckArtistExist(trackArtist.Name))
            {
                if (musicRepository.CheckArtistBelongsToTrack(trackArtist.TrackId, musicRepository.getArtist(trackArtist.Name).ArtistId))
                    return BadRequest("Artist is already part of the track");
            }

            var artists = musicRepository.GetAllArtistsFromTrack(trackArtist.TrackId);
            Artist artist = musicRepository.getArtist(trackArtist.Name);
            ArtistDto artistToAdd = new ArtistDto();
            if (artist is not null)
            {
                artistToAdd.ArtistId = artist.ArtistId;
                artistToAdd.Name = artist.Name;
            }
            else
            {
                artistToAdd.Name = trackArtist.Name;
            }

            artists.Add(artistToAdd);
            var track = musicRepository.getTrack(trackArtist.TrackId);

            if (!musicRepository.CheckTrackHasUniqueArtists(artists.Select(a => a.Name).ToList(), track.Titel))
                return BadRequest("Tracks with the same name must have unique artists");

            //todo: Case - Wenn letzter Track mit dem artist gelöscht wird, loesche artist auch
            musicRepository.AddArtistToTrack(trackArtist.TrackId, artistToAdd);
            return Ok();
        }
                
        [HttpDelete("{trackId}")]
        public IActionResult DeleteTrack(int trackId)
        {
            if (!musicRepository.CheckTrackExist(trackId))
                return NotFound("Track does not exist");

            //todo: Case - Wenn letzter Track mit dem artist gelöscht wird, lösche artist auch
            musicRepository.DeleteTrack(trackId);            
            return Ok();
        }

        [HttpDelete("track/artist")]
        public IActionResult DeleteArtistFromTrack([FromBody] DeleteArtistDto trackArtist)
        {
            if (!musicRepository.CheckTrackExist(trackArtist.TrackId))
                return NotFound("Track does not exist");

            if(!musicRepository.CheckArtistBelongsToTrack(trackArtist.TrackId, trackArtist.ArtistId))
                return BadRequest("Artist does not belongs to the track");

            var artists = musicRepository.GetAllArtistsFromTrack(trackArtist.TrackId);
            var artistToRemove = artists.Where(a => a.ArtistId == trackArtist.ArtistId).First();
            artists.Remove(artistToRemove);
            var track = musicRepository.getTrack(trackArtist.TrackId);

            if(!musicRepository.CheckTrackHasUniqueArtists(artists.Select(a => a.Name).ToList(), track.Titel))
                return BadRequest("Tracks with the same name must have unique artists");

            //todo: Case - Wenn letzter Track mit dem artist gelöscht wird, lösche artist auch
            musicRepository.DeleteArtistFromTrack(trackArtist.TrackId, trackArtist.ArtistId);

            return Ok();
        }

        
    }
}
