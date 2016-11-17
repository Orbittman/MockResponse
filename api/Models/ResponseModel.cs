using System.Collections.Generic;

namespace MockResponse
{
    public class ResponseModel
    {
        public int ResponseId { get; set; }

        public int StatusCode { get; set; }

        public string Content { get; set; }

        public string Path { get; set; }

        public List<HeaderModel> Headers { get; set; }
    }
}