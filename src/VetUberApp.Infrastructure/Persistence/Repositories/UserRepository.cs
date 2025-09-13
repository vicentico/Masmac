using MongoDB.Driver;
using VetUberApp.Domain.Entities;
using VetUberApp.Domain.Interfaces;

namespace VetUberApp.Infrastructure.Persistence.Repositories;

public class UserRepository : BaseRepository<User>, IUserRepository
{
    public UserRepository(MongoDbContext context) : base(context, "Users") { }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await Collection.Find(x => x.Email == email).FirstOrDefaultAsync();
    }

    public async Task<bool> ExistsAsync(string email)
    {
        return await Collection.Find(x => x.Email == email).AnyAsync();
    }
}