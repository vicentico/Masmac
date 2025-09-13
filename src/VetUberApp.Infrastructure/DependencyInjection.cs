using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VetUberApp.Infrastructure.Persistence;
using VetUberApp.Infrastructure.Persistence.Settings;

namespace VetUberApp.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var mongoDbSettings = new MongoDbSettings
        {
            ConnectionString = configuration.GetConnectionString("MongoDB") ?? throw new InvalidOperationException("MongoDB connection string not found."),
            DatabaseName = configuration.GetSection("MongoDb:DatabaseName").Get<string>() ?? "VetUberDB"
        };
        
        services.Configure<MongoDbSettings>(options =>
        {
            options.ConnectionString = mongoDbSettings.ConnectionString;
            options.DatabaseName = mongoDbSettings.DatabaseName;
        });

        services.AddSingleton<MongoDbContext>();

        return services;
    }
}