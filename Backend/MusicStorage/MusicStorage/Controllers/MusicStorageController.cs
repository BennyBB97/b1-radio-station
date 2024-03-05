using Microsoft.AspNetCore.Mvc;

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

       

        
    }
}
