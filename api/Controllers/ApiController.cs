using Microsoft.AspNetCore.Mvc;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using MockResponse.MongoDb;
using MongoDB.Driver;
using MongoDB.Bson;

namespace MockResponse
{
    public class ApiController : Controller
    {
        IMapper _mapper;

        private INoSqlClient _dbClient;

        public ApiController(IMapper mapper, INoSqlClient dbClient)
        {
            _mapper = mapper;
            _dbClient = dbClient;
        }

        [HttpGet("")]
        public void Index(){
            var response = new Response();
            var path = HttpContext.Request.Path.Value.TrimStart('/');
            if (path == string.Empty)
            {
                HttpContext.Response.StatusCode = 200;
                response.Content = "Mock Response - OK";
            }
            
            HttpContext.Response.WriteAsync(response?.Content ?? "Not found");
        }
        
        [HttpGet("responses")]
        public object GetResources()
        {
            var db = _dbClient.GetDatabase();
            var collection = db.GetCollection<Response>("Responses");
            var responses = collection.Find(new BsonDocument()).ToList(); //.Responses.Select(d => new ResponseModel
                // {
                //     StatusCode = d.StatusCode,
                //     ResponseId = d.Id,
                //     Path = d.Path,
                //     Content = d.Content,
                //     Headers = d.Headers.Select(h => new HeaderModel { Name = h.Name, Value = h.Value}).ToList()
                // }).ToArray();

                return Json(responses);
        }

        // [HttpDelete("responses/{responseId}")]
        // public object DeleteResources(int responseId)
        // {
        //     using (var db = new SqlLiteContext())
        //     {
        //         var response = db.Responses.SingleOrDefault(d => d.Id == responseId);
        //         if (response != null)
        //         {
        //             db.Remove(response);
        //             db.SaveChanges();

        //             return Json(responseId);
        //         } else {
        //             return NotFound();
        //         }
        //     }
        // }

        // [HttpPost("responses")]
        // public object PostResponses([FromBody]ResponseModel model)
        // {
        //     using (var db = new SqlLiteContext())
        //     {
        //         var domain = db.Domains.SingleOrDefault(d => d.Name == Request.Host.Host);
        //         if (domain == null) {
        //             domain = new Domain{ Name = Request.Host.Host };
        //         }

        //         var dbModel = new Response
        //         {
        //             StatusCode = model.StatusCode,
        //             Path = model.Path,
        //             Content = model.Content,
        //             Domain = domain,
        //             Headers = model.Headers.Select(h => new Header { Name = h.Name, Value = h.Value }).ToList()
        //         };

        //         var responses = db.Responses.Add(dbModel);
        //         db.SaveChanges();

        //         return Json(model);
        //     }
        // }
    }
}