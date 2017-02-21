using MongoDB.Driver;

public interface INoSqlClient
{
    IMongoDatabase GetDatabase();
}