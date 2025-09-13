using Microsoft.Extensions.Options;
using MongoDB.Driver;
using VetUberApp.Infrastructure.Persistence.Settings;

namespace VetUberApp.Infrastructure.Persistence;

public class MongoDbContext
{
    private readonly IMongoDatabase _database;

    public MongoDbContext(IOptions<MongoDbSettings> settings)
    {
        var mongoSettings = MongoClientSettings.FromUrl(new MongoUrl(settings.Value.ConnectionString));
        mongoSettings.AllowInsecureTls = true;
        mongoSettings.UseTls = true;
        mongoSettings.SslSettings = new SslSettings
        {
            EnabledSslProtocols = System.Security.Authentication.SslProtocols.Tls12
        };
        
        var client = new MongoClient(mongoSettings);
        _database = client.GetDatabase(settings.Value.DatabaseName);
    }

    public IMongoCollection<T> GetCollection<T>(string name)
    {
        return _database.GetCollection<T>(name);
    }
}