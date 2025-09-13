using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VetUberApp.Infrastructure.Persistence;
using VetUberApp.Infrastructure.Persistence.Settings;
using VetUberApp.Infrastructure.Persistence.Repositories;
using VetUberApp.Application.Services;
using VetUberApp.Domain.Interfaces;

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

        // Repositories
        services.AddScoped<IUserRepository, UserRepository>();

        // Services
        services.AddScoped<Application.Services.UserService>();

        return services;
    }
}