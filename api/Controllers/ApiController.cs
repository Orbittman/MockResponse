using System.Linq;
using System.Threading.Tasks;

using AutoMapper;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using MockResponse.Api.Commands;
using MockResponse.Api.Commands.Parameters;
using MockResponse.Api.Filters;
using MockResponse.Api.Models;
using MockResponse.Api.Queries;
using MockResponse.Api.Queries.Parameters;
using MockResponse.Core.Data;
using MockResponse.Core.Data.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace MockResponse.Api.Controllers
{
    public class ApiController : Controller
    {
        private readonly IMapper _mapper;
        private readonly INoSqlClient _dbClient;
        private readonly IRequestContext _requestContext;
        private readonly IResponseQuery _responseQuery;
        private readonly IResponseCommand _responseCommand;

        public ApiController(
            IMapper mapper, 
            INoSqlClient dbClient, 
            IRequestContext requestContext,
            IResponseQuery responseQuery,
            IResponseCommand responseCommand)
        {
            _responseQuery = responseQuery;
            _responseCommand = responseCommand;
            _requestContext = requestContext;
            _mapper = mapper;
            _dbClient = dbClient;
        }

        [HttpGet("responses")]
        [ServiceFilter(typeof(AuthorisationFilterAttribute))]
        public IActionResult GetResponses(int page = 1, int pageSize = 10)
        {
            var responses = _responseQuery.Execute(new ResponseParameters { Page = page, PageSize = pageSize });
            return Json(responses);
        }

        [HttpDelete("responses/{id}")]
        [ServiceFilter(typeof(AuthorisationFilterAttribute))]
        public IActionResult DeleteResponse(ResourceRequest request)
        {
            // Wrap all this in a command object
            var account = _dbClient.Find(Builders<Account>.Filter.Eq(a => a.ApiKey, _requestContext.ApiKey), nameof(Account)).SingleOrDefault();
            var filter = Builders<Response>.Filter
                                           .Eq(r => r.Id, new ObjectId(request.RequestId))
										   & Builders<Response>.Filter
										   .Eq(r => r.Account, account.Id);
            
            var deletedCount = _dbClient.DeleteOne(filter, nameof(Response));

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
            var response = _responseCommand.Execute(_mapper.Map<ResponsePostParameters>(model));
            return Json(response);
        }

        [HttpGet("{*url}")]
        public void Index()
        {
            var response = new Response();
            var content = "Not found";
            var path = HttpContext.Request.Path.Value.TrimStart('/');
            if (path == string.Empty)
            {
                HttpContext.Response.StatusCode = 200;
                response.Content = "Mock Response - OK";
            }
            else
            {
                var filter = Builders<Response>.Filter.Eq(r => r.Path, $"{path}");
                response = _dbClient.Find(filter, "Responses").FirstOrDefault();
                if (response != null)
                {
                    //if (response.Domains.Any(d => d.Host == Request.Host.Host))
                    //{
                    content = response.Content;
                    HttpContext.Response.StatusCode = response.StatusCode;
                    HttpContext.Response.ContentType = response.ContentType;
                    HttpContext.Response.Headers.Clear();
                    response.Headers?.ForEach(h => HttpContext.Response.Headers.Append(h.Name, h.Value));
                    //}
                }
            }

            Task.WaitAll(HttpContext.Response.WriteAsync(content));
        }
    }

    public class ResourceRequest
    {
        public string RequestId { get; set; }
    } 
}
