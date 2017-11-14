using System.Collections.Generic;

using MockResponse.Core.Data.Models;

namespace MockResponse.Api.Commands.Parameters
{
    public class ResponsePostParameters
    {
        public int StatusCode { get; set; }

        public List<Header> Headers { get; set; }

        public Domain Domain { get; set; }

        public string Content { get; set; }

        public string Path { get; set; }

        public string Id { get; set; }
    }
}
