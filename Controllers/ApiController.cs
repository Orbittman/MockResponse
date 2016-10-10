using Microsoft.AspNetCore.Mvc;
using System.Linq;
using AutoMapper;

namespace MockResponse{
public class ApiController : Controller {
        IMapper _mapper;

        public ApiController(IMapper mapper){
            _mapper = mapper;
        }

        [HttpGet("responses")]
        public object GetResources(){
            using(var db = new SqlLiteContext()){
                var responses = db.Responses.Select(d => new ResponseModel { StatusCode = d.StatusCode, ContentType = d.ContentType }).ToArray();
                return Json(responses);
            }
        }

        [HttpPost("responses")]
        public object PostResponses([FromBody]ResponseModel model)
        {
            using(var db = new SqlLiteContext()){
                var responses = db.Responses.Add(new Response{
                    ContentType=model.ContentType,
                    StatusCode = model.StatusCode
                });
                db.SaveChanges();
            }
            return new {ContentType=model.ContentType};
        }
    }
}