using System.Linq;
using System.Threading.Tasks;

using AutoMapper;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MockResponse.Api.Filters;
using MockResponse.Api.Models;
using MockResponse.Api.Queries;
using MockResponse.Core.Data;
using MockResponse.Core.Data.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace MockResponse.Api.Controllers
{
    public class ApiController : Controller
    {
        IMapper _mapper;

        private readonly INoSqlClient _dbClient;
        readonly IRequestContext _requestContext;
        readonly IResponseQuery _responseQuery;

        public ApiController(
            IMapper mapper, 
            INoSqlClient dbClient, 
            IRequestContext requestContext,
            IResponseQuery responseQuery)
        {
            _responseQuery = responseQuery;
            _requestContext = requestContext;
            _mapper = mapper;
            _dbClient = dbClient;
        }

        [HttpGet("responses")]
        [ServiceFilter(typeof(AuthorisationFilterAttribute))]
        public IActionResult GetResponses(int page = 1, int pageSize = 10)
        {
            var responses = _responseQuery.Execute(new ResponseParameters { Page = page, PageSize = pageSize }); // Page this
            return Json(responses);
        }

        [HttpDelete("responses/{id}")]
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
        public IActionResult PostResponse([FromBody] ResponseModel model)
        {
            var response = new Response
            {
                StatusCode = model.StatusCode,
                Path = model.Path,
                Content = model.Content,
                Domain = new Domain { Host = Request.Host.Host },
                Headers = model.Headers?.Select(h => new Header { Name = h.Name, Value = h.Value }).ToList()
            };

            _dbClient.InsertOne(response, "Responses");
            return Json(model);
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
