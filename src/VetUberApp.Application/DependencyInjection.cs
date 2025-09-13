using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using VetUberApp.Domain.Interfaces;
using VetUberApp.Application.Services;
using VetUberApp.Application.Interfaces;

namespace VetUberApp.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IVeterinarianService, VeterinarianService>();
        services.AddScoped<IAppointmentService, AppointmentService>();
        services.AddScoped<IReviewService, ReviewService>();
        services.AddScoped<IPetService, PetService>();
        services.AddScoped<IPaymentService, PaymentService>();
        
        return services;
    }
}