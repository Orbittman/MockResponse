using MongoDB.Driver;

public class MongoDbClient : INoSqlClient
{
    private MongoClient _client;

    private IMongoDatabase _db;

    public MongoDbClient(string connectionString){
        _client = new MongoClient(connectionString);
        _db = _client.GetDatabase("EmployeeDB");
    }
}