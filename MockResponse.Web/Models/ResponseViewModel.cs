using System.Collections.Generic;

namespace MockResponse.Web.Models
{
    public class ResponseViewModel
    {
        public string ResponseId { get; set; }
        
        public int StatusCode { get; set; }
        
        public string Content { get; set; }
        
        public string Path { get; set; }
        
        public List<HeaderViewModel> Headers { get; set; }
    }
}
