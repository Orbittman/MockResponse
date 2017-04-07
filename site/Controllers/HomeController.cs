using Microsoft.AspNetCore.Mvc;

namespace MockResponse.Site.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View("Index");
        }
    }
}