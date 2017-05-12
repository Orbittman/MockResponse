using System;

namespace MockResponse.Site.Controllers
{
    public class UserSession
    {
        public bool Authenticated { get; set; }

        public DateTime AuthenticatedAt { get; set; }

        public string AuthenticationToken { get; set; }
    }
}