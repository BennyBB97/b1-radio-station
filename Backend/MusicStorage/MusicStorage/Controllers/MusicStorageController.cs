using Microsoft.AspNetCore.Mvc;
using MusicStorage.Models;

namespace MusicStorage.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MusicStorageController : Controller
    {

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

       [HttpGet]
       [ProducesResponseType(200, Type = typeof(IEnumerable<Genre>))]
       public IActionResult GetGenres()
       {

       }

        
    }
}
