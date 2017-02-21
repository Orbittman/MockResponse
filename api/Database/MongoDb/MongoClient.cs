using MongoDB.Driver;

public class MongoDbClient : INoSqlClient
{
    private MongoClient _client;

    public MongoDbClient(string connectionString){
        _client = new MongoClient(connectionString);
    }

    public IMongoDatabase GetDatabase(){
        return _client.GetDatabase("IDL_Monitor");
    }
}