using System.Collections.Generic;
using MockResponse.MongoDb;
using MongoDB.Driver;

public interface INoSqlClient
{
    IEnumerable<TResponse> Find<TResponse>(FilterDefinition<TResponse> filter, string collectionName);

    IEnumerable<TResponse> Page<TResponse>(FilterDefinition<TResponse> filter, string collectionName, int page, int pageSize);
    void InsertOne<TResponse>(TResponse response, string collectionName);
}