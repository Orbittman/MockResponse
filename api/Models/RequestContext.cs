using Microsoft.AspNetCore.Http;

namespace MockResponse.Api.Models
{
    public class RequestContext : IRequestContext
    {
        const string _apiKeyHeader = "x-apikey";
        string _apiKey;
        readonly IHttpContextAccessor _httpContextAccessor;

        public RequestContext(IHttpContextAccessor httpContextAccessor){
            _httpContextAccessor = httpContextAccessor;
        }

        public string ApiKey => _apiKey ?? (_apiKey = _httpContextAccessor.HttpContext.Request.Headers[_apiKeyHeader]);

        public string PrimaryIdentity { get; set; }

        public string AccountId { get; set; }
    }
} 