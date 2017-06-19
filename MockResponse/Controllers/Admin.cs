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

        [HttpGet("responses")]
        public ActionResult List()
        {
            var responses = _apiClient.GetAsync<ResponsesRequest, ResponsesModel>(new ResponsesRequest());
            var model = responses.Result;
            return View(model);
        }

        [HttpGet("responses/{responseId}")]
        public ActionResult Edit()
        {
            var response = _apiClient.GetAsync<ResponseRequest, ResponseModel>(new ResponseRequest());
            var model = response.Result;
            return View(model);
        }

        [HttpPost("responses/{responseId}")]
        public ActionResult PostResponse(ResponseModel response)
        {
            return View();
        }
    }
}
