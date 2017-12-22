using System.Linq;
using System.Threading.Tasks;
using AutoMapper;

using Microsoft.AspNetCore.Mvc;

using MockResponse.Core.Models;
using MockResponse.Core.Requests;
using MockResponse.Web.ApiClient;
using MockResponse.Web.Models;

namespace MockResponse.Web.Controllers
{
    public class AdminController : BaseController
    {
        readonly IRestClient _apiClient;
        readonly IMapper _mapper;

        public AdminController(
            IRestClient apiClient, 
            IMapper mapper, 
            ISiteRequestContext requestContext, 
            IDomainContext domainContext)
            : base(requestContext, domainContext)
        {
            _apiClient = apiClient;
            _mapper = mapper;
        }

        [HttpGet("responses")]
        public ActionResult List()
        {
            var responses = _apiClient.GetAsync<ResponsesRequest, ResponsesModel>(new ResponsesRequest());

            var model = CreateViewModel<ListResponseViewModel>(m => m.Responses = responses.Result.Responses.Select(r => _mapper.Map<ResponseViewModel>(m))); 

            return View(model);
        }

        [HttpGet("responses/{responseId}")]
        public ActionResult Edit(string responseId)
        {
            ResponseModel model = null;

            if (!string.IsNullOrEmpty(responseId))
            {
                var response = _apiClient.GetAsync<ResponseRequest, ResponseModel>(new ResponseRequest { ResponseId = responseId });
                model = response.Result;
            }

            return View(new EditResponseViewModel{ Response = _mapper.Map<ResponseViewModel>(model ?? new ResponseModel()) });
        }

        [HttpPost("responses/{responseId}")]
        public async Task<ActionResult> PostResponse(ResponseViewModel response)
        {
            var model = _mapper.Map<ResponseModel>(response);
            await _apiClient.PostAsync<PostResponseRequest, ResponseModel>(new PostResponseRequest(model));

            return View("List");
        }
    }
}
