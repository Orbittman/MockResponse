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
                    ResponseId = d.Id,
                    Path = d.Path,
                    Content = d.Content,
                    Headers = d.Headers.Select(h => new HeaderModel { Name = h.Name, Value = h.Value}).ToList()
                }).ToArray();

                return Json(responses);
            }
        }

        [HttpDelete("responses/{responseId}")]
        public object DeleteResources(int responseId)
        {
            using (var db = new SqlLiteContext())
            {
                var response = db.Responses.SingleOrDefault(d => d.Id == responseId);
                if (response != null)
                {
                    db.Remove(response);
                    db.SaveChanges();

                    return Json(responseId);
                } else {
                    return NotFound();
                }
            }
        }

        [HttpPost("responses")]
        public object PostResponses([FromBody]ResponseModel model)
        {
            using (var db = new SqlLiteContext())
            {
                var domain = db.Domains.SingleOrDefault(d => d.Name == Request.Host.Host);
                if (domain == null) {
                    domain = new Domain{ Name = Request.Host.Host };
                }

                var dbModel = new Response
                {
                    StatusCode = model.StatusCode,
                    Path = model.Path,
                    Content = model.Content,
                    Domain = domain,
                    Headers = model.Headers.Select(h => new Header { Name = h.Name, Value = h.Value }).ToList()
                };

                var responses = db.Responses.Add(dbModel);
                db.SaveChanges();

                return Json(model);
            }
        }
    }
}