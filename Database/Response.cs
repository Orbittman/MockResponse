using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace MockResponse
{
    public class Response {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]        
        public int Id { get; set; }

        public Domain Domain { get; set; }

        public List<Header> Headers { get; set; }

        public int StatusCode { get; set; } 

        public string Content { get; set; }

        public string Path { get; set; }
    }
}