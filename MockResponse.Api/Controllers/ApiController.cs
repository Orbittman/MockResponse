using System.Linq;
using System.Text;

using AutoMapper;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using MockResponse.Api.Commands;
using MockResponse.Api.Commands.Parameters;
using MockResponse.Api.Filters;
using MockResponse.Api.Queries;
using MockResponse.Api.Queries.Parameters;
using MockResponse.Core.Data;
using MockResponse.Core.Data.Models;
using MockResponse.Core.Models;

using MongoDB.Driver;

namespace MockResponse.Api.Controllers
{
    public class ApiController : Controller
    {
        readonly IMapper _mapper;
        readonly INoSqlClient _dbClient;
        readonly IResponsesQuery _responsesQuery;
        private readonly IResponseQuery _responseQuery;
        readonly IPostResponseCommand _postResponseCommand;
        readonly IResponseDeleteCommand _responseDeleteCommand;

        public ApiController(
            IMapper mapper, 
            INoSqlClient dbClient,
            IResponsesQuery responsesQuery,
            IResponseQuery responseQuery,
            IPostResponseCommand postResponseCommand,
			IResponseDeleteCommand responseDeleteCommand)
        {
            _responseDeleteCommand = responseDeleteCommand;
            _responsesQuery = responsesQuery;
            _responseQuery = responseQuery;
            _postResponseCommand = postResponseCommand;
            _mapper = mapper;
            _dbClient = dbClient;
        }

        [HttpGet("responses")]
        [ServiceFilter(typeof(AuthorisationFilterAttribute))]
        public IActionResult GetResponses(int page = 1, int pageSize = 10)
        {
            var response = _responsesQuery.Execute(new ResponsesParameters { Page = page, PageSize = pageSize });
            var model = new ResponsesModel { Responses = response.Select(r => _mapper.Map<ResponseModel>(r)).ToList(), Name = "Timmy time" };
            return Json(model);
        }

        [HttpGet("responses/{responseid}")]
        [ServiceFilter(typeof(AuthorisationFilterAttribute))]
        public IActionResult GetResponse(ResponseRequest request)
        {
            var response =  _responseQuery.Execute(new ResponseParameters { ResponseId = request.ResponseId });

            if (response == null)
            {
                return NotFound();
            }

            return Json(response);
        }

        [HttpDelete("responses/{responseid}")]
        [ServiceFilter(typeof(AuthorisationFilterAttribute))]
        public IActionResult DeleteResponse(ResponseRequest request)
        {
            var deletedCount = _responseDeleteCommand.Execute(new ResponseDeleteParameters { ResponseId = request.ResponseId });

            if (deletedCount > 0)
            {
                return Json(deletedCount);
            }

            return NotFound();
        }

        [HttpPost("responses")]
        [ServiceFilter(typeof(AuthorisationFilterAttribute))]
        public IActionResult PostResponse([FromBody] ResponseModel model)
        {
            var response = _postResponseCommand.Execute(_mapper.Map<ResponsePostParameters>(model));
            return Json(response);
        }

        [HttpGet("{*url}")]
        public void Index()
        {
            var content = "Not found";
            var path = HttpContext.Request.Path.Value.TrimStart('/');
            if (path == string.Empty)
            {
                HttpContext.Response.StatusCode = 200;
                content = "Mock Response - OK";
            }
            else
            {
                var filter = Builders<Response>.Filter.Eq(r => r.Path, $"{path}");
                var response = _dbClient.Find(filter, nameof(Response)).FirstOrDefault();
                if (response != null)
                {
                    //if (response.Domains.Any(d => d.Host == Request.Host.Host))
                    //{
                    content = response.Content;
                    HttpContext.Response.StatusCode = response.StatusCode;
                    HttpContext.Response.ContentType = response.ContentType;
                    response.Headers?.ForEach(h => HttpContext.Response.Headers.Append(h.Name, h.Value));
                    //}
                }
            }

            HttpContext.Response.Body.WriteAsync(Encoding.UTF8.GetBytes(content), 0, content.Length);
        }
    }
}
