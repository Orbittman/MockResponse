// using System.Collections.Generic;

using System.ComponentModel.DataAnnotations.Schema;

namespace MockResponse
{
    public class Response {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]        
        public int ResponseId { get; set; }

        public string ContentType { get; set; }

        public string ContentEncoding { get; set; }

        public string Server { get; set; }

        public string Vary { get; set; }

        public string CacheControl { get; set; } 

        public int StatusCode { get; set; } 

        public string Content { get; set; }

        public string Path { get; set; }
    }
}