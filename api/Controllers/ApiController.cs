using System;
using System.Linq;
using System.Text;

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
using MockResponse.Core.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace MockResponse.Api.Controllers
{
    public class ApiController : Controller
    {
        readonly IMapper _mapper;
        readonly INoSqlClient _dbClient;
        readonly IRequestContext _requestContext;
        readonly IResponseQuery _responseQuery;
        readonly IResponseCommand _responseCommand;
        readonly IResponseDeleteCommand _responseDeleteCommand;

        public ApiController(
            IMapper mapper, 
            INoSqlClient dbClient, 
            IRequestContext requestContext,
			IResponseQuery responseQuery,
			IResponseCommand responseCommand,
			IResponseDeleteCommand responseDeleteCommand)
        {
            _responseDeleteCommand = responseDeleteCommand;
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
            var response = _responseQuery.Execute(new ResponseParameters { Page = page, PageSize = pageSize });
            var model = new ResponsesModel { Responses = response.Select(r => _mapper.Map<ResponseModel>(r)).ToList(), Name = "Timmy time" };
            return Json(model);
        }

        [HttpDelete("responses/{id}")]
        [ServiceFilter(typeof(AuthorisationFilterAttribute))]
        public IActionResult DeleteResponse(ResponseRequest request)
        {
            var deletedCount = _responseDeleteCommand.Execute(new ResponseDeleteParameters { RequestId = request.RequestId });

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
                content = "Mock Response - OK";
            }
            else
            {
                var filter = Builders<Response>.Filter.Eq(r => r.Path, $"{path}");
                response = _dbClient.Find(filter, nameof(Response)).FirstOrDefault();
                if (response != null)
                {
                    //if (response.Domains.Any(d => d.Host == Request.Host.Host))
                    //{
                    var content2 = Encoding.UTF8.GetBytes(response.Content);
                    HttpContext.Response.StatusCode = response.StatusCode;
                    Response.Body.Write(content2, 0, content2.Length);
                    //HttpContext.Response.ContentType = response.ContentType;
                    //HttpContext.Response.Headers.Clear();
                    //response.Headers?.ForEach(h => HttpContext.Response.Headers.Append(h.Name, h.Value));
                    //}
                }
            }

            //await HttpContext.Response.Body.WriteAsync(Encoding.UTF8.GetBytes(content), 0, );
        }
    }
}
