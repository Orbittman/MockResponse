using System.Collections.Generic;

using MongoDB.Driver;

namespace MockResponse.Core.Data
{
    public class MongoDbClient : INoSqlClient
    {
        private readonly MongoClient _client;

        public MongoDbClient(string connectionString)
        {
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

        public void UpdateOne<TResponse>(FilterDefinition<TResponse> filter, UpdateDefinition<TResponse> update, string collectionName)
        {
            GetCollection<TResponse>(collectionName).UpdateOne(filter, update);
        }

        public long DeleteOne<TResponse>(FilterDefinition<TResponse> filter, string collectionName)
        {
            var result = GetCollection<TResponse>(collectionName).DeleteOne(filter);
            return result.DeletedCount;
        }

        private IFindFluent<TResponse, TResponse> GetQuery<TResponse>(FilterDefinition<TResponse> filter, string collectionName)
        {
            return GetCollection<TResponse>(collectionName).Find(filter);
        }

        private IMongoDatabase GetDatabase()
        {
            return _client.GetDatabase("IDL_Monitor");
        }

        private IMongoCollection<TCollectionType> GetCollection<TCollectionType>(string collectionName)
        {
            return GetDatabase().GetCollection<TCollectionType>(collectionName);
        }
    }
}
