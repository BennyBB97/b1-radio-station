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


        [HttpGet("test")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Track>))]
        public IActionResult SearchTrack(SearchTracksDto searchTracks)
        {
            searchTracks.SearchTerm = "Taylor Swift";
            if (searchTracks.SearchTerm is null)
                return BadRequest(ModelState);


            var tracks = musicRepository.SearchTracks(searchTracks);

            return Ok(tracks);
        }
    }
}
