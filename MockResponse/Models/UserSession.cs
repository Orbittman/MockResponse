using System;

namespace MockResponse.Web.Models
{
    public class UserSession
    {
        public bool Authenticated { get; set; }

        public DateTime AuthenticatedAt { get; set; }

        public string AuthenticationToken { get; set; }

        public string Name { get; set; }

        public string Identity { get; set; }
    }
}