using System.Collections.Generic;
using MongoDB.Bson;

namespace MockResponse.MongoDb
{
    public class Response {    
        public ObjectId Id { get; set; }

        public Domain Domain { get; set; }

        public List<Header> Headers { get; set; }

        public int StatusCode { get; set; } 

        public string Content { get; set; }

        public string Path { get; set; }
    }

    public class Domain{

    }

    public class Header{

    }
}