using System;
using Microsoft.AspNetCore.Http;

namespace MockResponse.Web.Models
{
    public interface ISiteRequestContext
    {
        (string, TimeSpan) SaveUserSession(UserSession userSession);

        bool Authenticated { get; }

        UserSession Session { get; }

        string ApiKey { get; }

        HttpContext HttpContext { get; }

        void ClearSession();
    }
}