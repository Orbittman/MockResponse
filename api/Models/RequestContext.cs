using Microsoft.AspNetCore.Http;

namespace MockResponse.Api
{
    public class RequestContext : IRequestContext
    {
        const string apiKeyHeader = "x-apikey";
        string _apiKey;
        IHttpContextAccessor _httpContextAccessor;

        internal RequestContext(IHttpContextAccessor httpContextAccessor){
            _httpContextAccessor = httpContextAccessor;
        }

        public string ApiKey {get{
                return _apiKey ?? (_apiKey = _httpContextAccessor.HttpContext.Request.Headers[apiKeyHeader]);
            }}
    }
} 