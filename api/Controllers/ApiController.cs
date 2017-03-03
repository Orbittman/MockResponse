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
        
        [HttpGet("responses")]
        public object GetResources()
        {
            var responses = _dbClient.Page<Response>(new BsonDocument(), "Responses", 1, 10);
            return Json(responses);
        }

        // [HttpDelete("responses/{responseId}")]
        // public object DeleteResources(int responseId)
        // {
           
            // using (var db = new SqlLiteContext())
            // {
            //     var response = db.Responses.SingleOrDefault(d => d.Id == responseId);
            //     if (response != null)
            //     {
            //         db.Remove(response);
            //         db.SaveChanges();

            //         return Json(responseId);
            //     } else {
            //         return NotFound();
            //     }
            // }
        //}

        [HttpPost("responses")]
        public object PostResponses([FromBody]ResponseModel model)
        {
            var response = new Response
                {
                    StatusCode = model.StatusCode,
                    Path = model.Path,
                    Content = model.Content,
                    Domain = new Domain() { Host = Request.Host.Host},
                    Headers = model.Headers?.Select(h => new Header { Name = h.Name, Value = h.Value }).ToList()
                };

            _dbClient.InsertOne(response, "Responses");
            return  Json(model);
        }

        [HttpGet("{*url}")]
        public void Index(){
            var response = new Response();
            var content = "Not found";
            var path = HttpContext.Request.Path.Value.TrimStart('/');
            if (path == string.Empty)
            {
                HttpContext.Response.StatusCode = 200;
                response.Content = "Mock Response - OK";
            }else{
                var filter = Builders<Response>.Filter.Eq(r => r.Path, $"{path}");
                response = _dbClient.Find<Response>(filter, "Responses").SingleOrDefault();
                if(response != null){
                    content = response.Content;
                    HttpContext.Response.StatusCode = response.StatusCode;
                    HttpContext.Response.Headers.Clear();
                    if(response.Headers != null)
                    {
                        response.Headers.ForEach(h => HttpContext.Response.Headers.Append(h.Name, h.Value));
                    }
                }
            }

            HttpContext.Response.WriteAsync(content);
        }
    }
}