using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VetUberApp.Infrastructure.Persistence;
using VetUberApp.Infrastructure.Persistence.Settings;

namespace VetUberApp.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var mongoDbSettings = configuration.GetSection("MongoDb").Get<MongoDbSettings>();
        services.Configure<MongoDbSettings>(options =>
        {
            options.ConnectionString = mongoDbSettings?.ConnectionString ?? "mongodb://localhost:27017";
            options.DatabaseName = mongoDbSettings?.DatabaseName ?? "VetUberDB";
        });

        services.AddSingleton<MongoDbContext>();

        return services;
    }
}