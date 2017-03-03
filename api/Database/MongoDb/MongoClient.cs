using System;
using System.Collections.Generic;
using MockResponse.MongoDb;
using MongoDB.Driver;

public class MongoDbClient : INoSqlClient
{
    private MongoClient _client;

    public MongoDbClient(string connectionString){
        _client = new MongoClient(connectionString);
    }

    public IEnumerable<TResponse> Find<TResponse>(FilterDefinition<TResponse> filter, string collectionName)
    {
        return GetQuery(filter, collectionName).ToEnumerable();
    }

    public IEnumerable<TResponse> Page<TResponse>(FilterDefinition<TResponse> filter, string collectionName, int page, int pageSize)
    {
        return GetQuery(filter, collectionName).Skip((page - 1) * pageSize).Limit(pageSize).ToEnumerable();
    }
    public void InsertOne<TResponse>(TResponse response, string collectionName)
    {
        GetCollection<TResponse>(collectionName).InsertOne(response);
    }

    private IFindFluent<TResponse, TResponse> GetQuery<TResponse>(FilterDefinition<TResponse> filter, string collectionName){
        return GetCollection<TResponse>(collectionName).Find(filter);
    }

    private IMongoDatabase GetDatabase(){
        return _client.GetDatabase("IDL_Monitor");
    }

    private IMongoCollection<TCollectionType> GetCollection<TCollectionType>(string collectionName){
        return GetDatabase().GetCollection<TCollectionType>(collectionName);
    }
}