﻿using MongoDB.Bson;

namespace MockResponse.Api
{
    public interface IRequestContext
    {
        string ApiKey { get; }

        string PrimaryIdentity { get; set; }

        string AccountId { get; set; }
    }
}