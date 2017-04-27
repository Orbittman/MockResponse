using System.Linq;
using System.Threading.Tasks;

using AutoMapper;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using MockResponse.Api.Database.MongoDb;
using MockResponse.Api.Models;
using MockResponse.Core.Data;

using MongoDB.Bson;
using MongoDB.Driver;

namespace MockResponse.Api.Controllers
{
    public class ApiController : Controller
    {
        IMapper _mapper;

        private readonly INoSqlClient _dbClient;

        public ApiController(IMapper mapper, INoSqlClient dbClient)
        {
            _mapper = mapper;
            _dbClient = dbClient;
        }

        [HttpGet("responses")]
        public IActionResult GetResources()
        {
            var responses = _dbClient.Page<Response>(new BsonDocument(), "Responses", 1, 10);
            return Json(responses);
        }

        [HttpDelete("responses/{id}")]
        public IActionResult DeleteResources(string id)
        {
            var filter = Builders<Response>.Filter.Eq(r => r.Id, new ObjectId(id));
            var deletedCount = _dbClient.DeleteOne(filter, "Responses");

            if (deletedCount > 0)
            {
                return Json(deletedCount);
            }

            return NotFound();
        }

        [HttpPost("responses")]
        public IActionResult PostResponses([FromBody] ResponseModel model)
        {
            var response = new Response
            {
                StatusCode = model.StatusCode,
                Path = model.Path,
                Content = model.Content,
                Domain = new Domain() { Host = Request.Host.Host },
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
}
