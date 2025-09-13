using MongoDB.Driver;
using VetUberApp.Domain.Entities;
using VetUberApp.Domain.Interfaces;

namespace VetUberApp.Infrastructure.Persistence.Repositories;

public class PetRepository : BaseRepository<Pet>, IPetRepository
{
    public PetRepository(MongoDbContext context) : base(context, "Pets") { }

    public async Task<IEnumerable<Pet>> GetByOwnerIdAsync(string ownerId)
    {
        return await Collection.Find(x => x.OwnerId == ownerId && !x.IsDeleted)
            .ToListAsync();
    }

    public async Task<bool> BelongsToOwnerAsync(string petId, string ownerId)
    {
        return await Collection.Find(x => x.Id == petId && x.OwnerId == ownerId && !x.IsDeleted)
            .AnyAsync();
    }

    public async Task<IEnumerable<Pet>> FindByTypeAsync(string type)
    {
        return await Collection.Find(x => x.Type.ToString() == type && !x.IsDeleted)
            .ToListAsync();
    }

    public override async Task<IEnumerable<Pet>> GetAllAsync()
    {
        return await Collection.Find(x => !x.IsDeleted)
            .ToListAsync();
    }
}