using System.Collections.Generic;

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MockResponse.Core.Data.Models
{
    public class Response : IAccountEntity
    {
        public ObjectId Id { get; set; }

        public ObjectId Account { get; set; }

        public Domain Domain { get; set; }

        public List<Header> Headers { get; set; }

        public int StatusCode { get; set; }

        public string Content { get; set; }

        public string Path { get; set; }

        public string ContentType { get; internal set; }
    }

    public class Domain
    {
        public string Host { get; set; }
    }

    public class Header
    {
        public string Name { get; set; }

        public string Value { get; set; }
    }
}
