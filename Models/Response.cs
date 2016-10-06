// using System.Collections.Generic;

namespace MockResponse
{
    public class Response {
        public int ResponseId { get; set; }

        public string ContentType { get; set; }

        public string ContentEncoding { get; set; }

        public string Server { get; set; }

        public string Vary { get; set; }

        public string CacheControl { get; set; } 

        public int HttpStatusCode { get; set; } 

        // public List<string> Cookies { get; set; }

        public string Content { get; set; }
    }
}