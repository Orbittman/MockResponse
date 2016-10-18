using System.Collections.Generic;

namespace MockResponse {
    public class Domain {

        public int Id { get; set; }

        public string Name { get; set; }

        public List<Response> Responses { get; set; }
    }
}