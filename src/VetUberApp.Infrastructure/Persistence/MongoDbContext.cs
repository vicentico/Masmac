using Microsoft.Extensions.Options;
using MongoDB.Driver;
using VetUberApp.Infrastructure.Persistence.Settings;

namespace VetUberApp.Infrastructure.Persistence;

public class MongoDbContext
{
    protected readonly IMongoClient? _client;
    private readonly IMongoDatabase? _database;

    public MongoDbContext() { } // Constructor sin parámetros para mocking

    public MongoDbContext(IOptions<MongoDbSettings> settings)
    {
        var mongoSettings = MongoClientSettings.FromUrl(new MongoUrl(settings.Value.ConnectionString));
        mongoSettings.ServerApi = new ServerApi(ServerApiVersion.V1);
        mongoSettings.UseTls = true;

        _client = new MongoClient(mongoSettings);
        _database = _client.GetDatabase(settings.Value.DatabaseName);
    }

    public virtual IMongoCollection<T> GetCollection<T>(string name)
    {
        if (_database == null)
            throw new InvalidOperationException("Database not initialized. Use parameterized constructor for actual database operations.");
            
        return _database.GetCollection<T>(name);
    }
    
    public virtual IMongoDatabase GetDatabase()
    {
        if (_database == null)
            throw new InvalidOperationException("Database not initialized. Use parameterized constructor for actual database operations.");
            
        return _database;
    }
}