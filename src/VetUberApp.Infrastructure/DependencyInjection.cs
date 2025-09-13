using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using VetUberApp.Infrastructure.Persistence;
using VetUberApp.Infrastructure.Persistence.Settings;
using VetUberApp.Infrastructure.Persistence.Repositories;
using VetUberApp.Domain.Interfaces;
using VetUberApp.Application.Services;
using VetUberApp.Application.Interfaces;

namespace VetUberApp.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<MongoDbSettings>(options =>
        {
            options.ConnectionString = configuration.GetSection("MongoDb:ConnectionString").Value ?? 
                throw new InvalidOperationException("MongoDB connection string not found.");
            options.DatabaseName = configuration.GetSection("MongoDb:DatabaseName").Value ?? 
                throw new InvalidOperationException("MongoDB database name not found.");
        });

        services.AddSingleton<MongoDbContext>();

        // Repositories
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IVeterinarianRepository, VeterinarianRepository>();
        services.AddScoped<IPetRepository, PetRepository>();
        services.AddScoped<IAppointmentRepository, AppointmentRepository>();
        services.AddScoped<IReviewRepository, ReviewRepository>();
        services.AddScoped<IPaymentRepository, PaymentRepository>();

        // Services
        services.AddScoped<IReviewService, ReviewService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IVeterinarianService, VeterinarianService>();
        services.AddScoped<IPetService, PetService>();
        services.AddScoped<IAppointmentService, AppointmentService>();

        return services;
    }
}