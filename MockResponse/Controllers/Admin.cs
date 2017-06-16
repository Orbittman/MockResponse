using Microsoft.AspNetCore.Mvc;

using MockResponse.Core.Models;
using MockResponse.Web.ApiClient;

namespace MockResponse.Web.Controllers
{
    public class AdminController : Controller
    {
        readonly IRestClient _apiClient;

        public AdminController(IRestClient apiClient)
        {
            _apiClient = apiClient;
        }

        [HttpGet("list")]
        public ActionResult List()
        {
            var responses = _apiClient.GetAsync<ResponsesRequest, ResponsesModel>(new ResponsesRequest());
            var model = responses.Result;
            return View(model);
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
