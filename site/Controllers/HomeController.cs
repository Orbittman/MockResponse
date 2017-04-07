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

        [HttpPost]
        public IActionResult Login(string emailAddress)
        {
            return View("Login");
        }
    }
}