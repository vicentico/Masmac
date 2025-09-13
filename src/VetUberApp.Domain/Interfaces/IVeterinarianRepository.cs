using VetUberApp.Domain.Entities;

namespace VetUberApp.Domain.Interfaces;

public interface IVeterinarianRepository : IBaseRepository<Veterinarian>
{
    Task<Veterinarian?> GetByEmailAsync(string email);
    Task<bool> ExistsAsync(string email);
    Task<IEnumerable<Veterinarian>> GetAvailableAsync();
    Task<IEnumerable<Veterinarian>> FindBySpecialtyAsync(string specialty);
    Task UpdateLocationAsync(string id, double latitude, double longitude);
    Task UpdateAvailabilityAsync(string id, bool isAvailable);
}