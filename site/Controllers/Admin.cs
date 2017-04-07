using Microsoft.AspNetCore.Mvc;
using MockResponse.Site.Models;

namespace MockResponse.Site.Controllers
{
    public class AdminController : Controller
    {
        [HttpGet("list")]
        public ActionResult List()
        {
            return View();
        }

        [HttpGet("{responseId}/edit")]
        public ActionResult EditResponse()
        {
            return View();
        }

        [HttpPost("{responseId}/edit")]
        public ActionResult PostResponse(ResponseModel response)
        {
            return View();
        }
    }
}