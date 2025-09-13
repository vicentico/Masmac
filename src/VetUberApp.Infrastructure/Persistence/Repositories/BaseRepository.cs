using System.Linq.Expressions;
using MongoDB.Driver;
using VetUberApp.Domain.Common;

namespace VetUberApp.Infrastructure.Persistence.Repositories;

public abstract class BaseRepository<T> where T : BaseEntity
{
    protected readonly IMongoCollection<T> Collection;

    protected BaseRepository(MongoDbContext context, string collectionName)
    {
        Collection = context.GetCollection<T>(collectionName);
    }

    public virtual async Task<T> GetByIdAsync(string id)
    {
        return await Collection.Find(x => x.Id == id).FirstOrDefaultAsync();
    }

    public virtual async Task<IEnumerable<T>> GetAllAsync()
    {
        return await Collection.Find(_ => true).ToListAsync();
    }

    public virtual async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
    {
        return await Collection.Find(predicate).ToListAsync();
    }

    public virtual async Task<T> AddAsync(T entity)
    {
        entity.CreatedAt = DateTime.UtcNow;
        await Collection.InsertOneAsync(entity);
        return entity;
    }

    public virtual async Task<bool> UpdateAsync(T entity)
    {
        entity.UpdatedAt = DateTime.UtcNow;
        var result = await Collection.ReplaceOneAsync(x => x.Id == entity.Id, entity);
        return result.ModifiedCount > 0;
    }

    public virtual async Task<bool> DeleteAsync(string id)
    {
        var result = await Collection.UpdateOneAsync(
            x => x.Id == id,
            Builders<T>.Update.Set(x => x.IsDeleted, true)
                            .Set(x => x.UpdatedAt, DateTime.UtcNow));
        return result.ModifiedCount > 0;
    }

    public virtual async Task<bool> HardDeleteAsync(string id)
    {
        var result = await Collection.DeleteOneAsync(x => x.Id == id);
        return result.DeletedCount > 0;
    }
}