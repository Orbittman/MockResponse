using MongoDB.Bson;

namespace MockResponse.Core.Data.Models
{
    public class Account
    {
        public ObjectId Id { get; set; }

        public string PrimaryIdentity { get; set; }

        public string[] ApiKeys { get; set; }
    }
}