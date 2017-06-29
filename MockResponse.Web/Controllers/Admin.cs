using AutoMapper;

using Microsoft.AspNetCore.Mvc;

using MockResponse.Core.Models;
using MockResponse.Core.Requests;
using MockResponse.Web.ApiClient;
using MockResponse.Web.Models;

namespace MockResponse.Web.Controllers
{
    public class AdminController : Controller
    {
        readonly IRestClient _apiClient;
        private readonly IMapper _mapper;

        public AdminController(IRestClient apiClient, IMapper mapper)
        {
            _apiClient = apiClient;
            _mapper = mapper;
        }

        [HttpGet("responses")]
        public ActionResult List()
        {
            var responses = _apiClient.GetAsync<ResponsesRequest, ResponsesModel>(new ResponsesRequest());
            var model = responses.Result;

            return View(model);
        }

        [HttpGet("responses/{responseId}")]
        public ActionResult Edit(string responseId)
        {
            var response = _apiClient.GetAsync<ResponseRequest, ResponseModel>(new ResponseRequest { ResponseId = responseId });
            var model = response.Result;

            return View(new EditResponseViewModel{ Response = _mapper.Map<ResponseViewModel>(model) });
        }

        [HttpPost("responses/{responseId}")]
        public ActionResult PostResponse(ResponseViewModel response)
        {
            var model = _mapper.Map<ResponseModel>(response);
            _apiClient.PostAsync<PostResponseRequest, ResponseModel>(new PostResponseRequest(model));

            return View("List");
        }
    }
}
