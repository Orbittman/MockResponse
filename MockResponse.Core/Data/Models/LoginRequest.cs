using System;

using MongoDB.Bson;

namespace MockResponse.Core.Data.Models
{
    public class LoginRequest
    {
        public ObjectId Id { get; set; }

        public string AuthIdentity { get; set; }

        public string Token { get; set; }

        public DateTime TimeStamp { get; set; }
    }
}
