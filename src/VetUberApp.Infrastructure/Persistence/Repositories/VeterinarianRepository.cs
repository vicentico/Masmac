using MongoDB.Driver;
using VetUberApp.Domain.Entities;
using VetUberApp.Domain.Interfaces;
using VetUberApp.Domain.ValueObjects;

namespace VetUberApp.Infrastructure.Persistence.Repositories;

public class VeterinarianRepository : BaseRepository<Veterinarian>, IVeterinarianRepository
{
    public VeterinarianRepository(MongoDbContext context) : base(context, "Veterinarians") { }

    public async Task<Veterinarian?> GetByEmailAsync(string email)
    {
        return await Collection.Find(x => x.Email == email).FirstOrDefaultAsync();
    }

    public async Task<bool> ExistsAsync(string email)
    {
        return await Collection.Find(x => x.Email == email).AnyAsync();
    }

    public async Task<IEnumerable<Veterinarian>> GetAvailableAsync()
    {
        return await Collection.Find(x => x.IsAvailable && !x.IsDeleted).ToListAsync();
    }

    public async Task<IEnumerable<Veterinarian>> FindBySpecialtyAsync(string specialty)
    {
        return await Collection.Find(x => x.Specialties.Contains(specialty) && !x.IsDeleted).ToListAsync();
    }

    public async Task UpdateLocationAsync(string id, double latitude, double longitude)
    {
        var update = Builders<Veterinarian>.Update
            .Set(x => x.CurrentLocation.Latitude, latitude)
            .Set(x => x.CurrentLocation.Longitude, longitude)
            .Set(x => x.UpdatedAt, DateTime.UtcNow);

        await Collection.UpdateOneAsync(x => x.Id == id, update);
    }

    public async Task UpdateAvailabilityAsync(string id, bool isAvailable)
    {
        var update = Builders<Veterinarian>.Update
            .Set(x => x.IsAvailable, isAvailable)
            .Set(x => x.UpdatedAt, DateTime.UtcNow);

        await Collection.UpdateOneAsync(x => x.Id == id, update);
    }
}