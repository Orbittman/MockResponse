using System;

using Microsoft.AspNetCore.Http;

using MockResponse.Core.Caching;

namespace MockResponse.Site.Controllers
{
    public class SiteRequestContext : ISiteRequestContext
    {
        private const string _cacheKey = "mr-sid";
        private readonly ICacheClient _cacheClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        private UserSession _userSession;

        public SiteRequestContext(ICacheClient cacheClient, IHttpContextAccessor httpContextAccessor)
        {
            _cacheClient = cacheClient;
            _httpContextAccessor = httpContextAccessor;
        }

        public UserSession Session => _userSession ?? (_userSession = LoadSession());

        public void SaveUserSession(UserSession session)
        {
            var sessionKey = _httpContextAccessor.HttpContext.Request.Cookies[_cacheKey];
            if (string.IsNullOrEmpty(sessionKey))
            {
                sessionKey = Guid.NewGuid().ToString();
                _httpContextAccessor.HttpContext.Response.Cookies.Append(_cacheKey, sessionKey);
            }
            _cacheClient.Set(sessionKey, session);
        }

        public bool Authenticated => Session.Authenticated;

        private UserSession LoadSession()
        {
            return _cacheClient.Get<UserSession>(_httpContextAccessor.HttpContext.Request.Cookies[_cacheKey]) ?? new UserSession();
        }
    }
}