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

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Genre>))]
        public IActionResult GetGenres()
        {
            var genres = musicRepository.GetGenres();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var tracks = musicRepository.SearchTracks(new SearchTracksDto { });
            return Ok(genres);
        }

        [HttpGet("AllTracks")]
        public IActionResult GetAllTracks()
        {
            var tracks = musicRepository.GetAllTracks();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            return Ok(tracks);
        }

        [HttpPost("createTrack")]
        public IActionResult CreateTrack([FromBody] CreateTrackDto track)
        {
            if (track.ArtistNames.Count == 0)
                return BadRequest("No artist was specified");

            if(track.Titel.Equals("") || track.Titel.Length >= 50)
                return BadRequest("The title must contain 1 to 50 characters");

            if(!musicRepository.CheckGenreExist(track.GenreId))
                return BadRequest("The genre does not exist");

            if (musicRepository.CheckTrackExist(track.Titel))
            {
                if (!musicRepository.CheckTrackHasUniqueArtists(track.ArtistNames, track.Titel))
                {
                    ModelState.AddModelError("", "Track titel already exist and musst have unique artists");
                }
            }
            
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            musicRepository.CreateTrack(track);
            return Ok("Successfully created");
        }

        [HttpDelete("{trackId}")]
        public IActionResult DeleteTrack(int trackId)
        {
            if (!musicRepository.CheckTrackExist(trackId))
                return NotFound();

            //todo: Case - Wenn letzter Track mit dem artist gelöscht wird, loesche artist auch
            musicRepository.DeleteTrack(trackId);

            return Ok();
        }

        [HttpDelete("")]
        public IActionResult DeleteArtistFromTrack(ModifyArtistsDto artists)
        {
            if (musicRepository.CheckTrackExist(artists.TrackId))
                return NotFound("Track does not exist");

            //todo: Case - Wenn letzter Track mit dem artist gelöscht wird, loesche artist auch
            musicRepository.DeleteArtistFromTrack(artists);

            return Ok();
        }


        [HttpGet("SearchTrack")]
        public IActionResult SearchTrack(SearchTracksDto searchTracks)
        {
            searchTracks.SearchTerm = "Taylor Swift";
            if (searchTracks.SearchTerm is null)
                return BadRequest(ModelState);

            var tracks = musicRepository.SearchTracks(searchTracks);

            return Ok(tracks);
        }

        [HttpPut("")]        
        public IActionResult UpdateTrack([FromBody] UpdateTrackDto track)
        {  
            if(musicRepository.CheckTrackExist(track.TrackId))
                return BadRequest("Track does not exist");

            if (musicRepository.CheckGenreExist(track.GenreId))
                return BadRequest("Genre does not exist");

            musicRepository.UpdateTrack(track);
            return Ok();
        }
    }
}
