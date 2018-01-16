using System;
using MongoDB.Bson;

namespace MockResponse.Core.Data.Models
{
    public class SessionRecord
    {
        public ObjectId Account { get; set; }
        public string SessionKey { get; set; }
        public DateTime Expiry { get; set; }
    }
}