using System;
using MongoDB.Bson;

namespace MockResponse.Core.Requests
{
    public class SaveSessionRecordRequest
    {
        public ObjectId AccountId { get; set; }

        public string SessionKey { get; set; }

        public TimeSpan Expiry { get; set; }
    }
}