using System;
using System.Linq;
using Microsoft.AspNetCore.Http;

using MockResponse.Core.Caching;
using MockResponse.Core.Data;
using MockResponse.Core.Data.Models;
using MongoDB.Driver;

namespace MockResponse.Site.Models
{
    public class SiteRequestContext : ISiteRequestContext
    {
        const string _cacheKey = "mr-sid";
        readonly ICacheClient _cacheClient;
        readonly IHttpContextAccessor _httpContextAccessor;

        UserSession _userSession;
        string _apiKey;
        readonly INoSqlClient _dbClient;

        public SiteRequestContext(
            ICacheClient cacheClient, 
            IHttpContextAccessor httpContextAccessor,
            INoSqlClient dbClient)
        {
            _dbClient = dbClient;
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

        public string ApiKey => _apiKey ?? (_apiKey = LoadApiKey());

        private string LoadApiKey()
        {
			var account = _dbClient.Find(Builders<Account>.Filter.Eq(r => r.PrimaryIdentity, Session.Identity), nameof(Account))
								   .FirstOrDefault();
            return account?.ApiKeys.FirstOrDefault();
        }

        private UserSession LoadSession()
        {
            return _cacheClient.Get<UserSession>(_httpContextAccessor.HttpContext.Request.Cookies[_cacheKey]) ?? new UserSession();
        }

        public void ClearSession()
        {
            _cacheClient.Delete(_httpContextAccessor.HttpContext.Request.Cookies[_cacheKey]);
            _httpContextAccessor.HttpContext.Response.Cookies.Delete(_cacheKey);
        }
    }
}