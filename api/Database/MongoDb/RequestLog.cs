using System;
using MongoDB.Bson;

public class RequestLog {
    private string host;

    public ObjectId DomainId { get; set; }

    public string Path { get; set; }

    public string ClientIP { get; set; }

    public DateTime TimeStamp { get; set; }
}