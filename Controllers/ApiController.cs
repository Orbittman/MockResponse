using Microsoft.AspNetCore.Mvc;
using System.Linq;
using AutoMapper;

namespace MockResponse
{
    public class ApiController : Controller
    {
        IMapper _mapper;

        public ApiController(IMapper mapper)
        {
            _mapper = mapper;
        }

        [HttpGet("responses")]
        public object GetResources()
        {
            using (var db = new SqlLiteContext())
            {
                var responses = db.Responses.Select(d => new ResponseModel
                {
                    StatusCode = d.StatusCode,
                    ContentType = d.ContentType,
                    ResponseId = d.ResponseId,
                    Path = d.Path,
                    Content = d.Content,
                    Vary = d.Vary,
                    Server = d.Server
                }).ToArray();

                return Json(responses);
            }
        }

        [HttpDelete("responses/{responseId}")]
        public object DeleteResources(int responseId)
        {
            using (var db = new SqlLiteContext())
            {
                var response = db.Responses.SingleOrDefault(d => d.ResponseId == responseId);
                if (response != null)
                {
                    db.Remove(response);
                    db.SaveChanges();
                }
                
                return Json(responseId);
            }
        }

        [HttpPost("responses")]
        public object PostResponses([FromBody]ResponseModel model)
        {
            var dbModel = new Response
            {
                ContentType = model.ContentType,
                StatusCode = model.StatusCode,
                Path = model.Path,
                Content = model.Content,
                CacheControl = model.CacheControl,
                Vary = model.Vary,
                Server = model.Server
            };

            using (var db = new SqlLiteContext())
            {
                var responses = db.Responses.Add(dbModel);
                db.SaveChanges();
            }

            return Json(model);
        }
    }
}