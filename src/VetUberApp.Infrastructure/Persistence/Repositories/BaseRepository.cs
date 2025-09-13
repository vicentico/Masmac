using System.Linq.Expressions;
using MongoDB.Bson;
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

    public virtual async Task<T> CreateAsync(T entity)
    {
        if (string.IsNullOrEmpty(entity.Id))
        {
            entity.Id = ObjectId.GenerateNewId().ToString();
        }
        entity.CreatedAt = DateTime.UtcNow;
        await Collection.InsertOneAsync(entity);
        return entity;
    }

    public virtual async Task<T?> GetByIdAsync(string id)
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

    public virtual async Task<T> UpdateAsync(T entity)
    {
        entity.UpdatedAt = DateTime.UtcNow;
        await Collection.ReplaceOneAsync(x => x.Id == entity.Id, entity);
        return entity;
    }

    public virtual async Task DeleteAsync(string id)
    {
        await Collection.UpdateOneAsync(
            x => x.Id == id,
            Builders<T>.Update.Set(x => x.IsDeleted, true)
                            .Set(x => x.UpdatedAt, DateTime.UtcNow));
    }

    public virtual async Task HardDeleteAsync(string id)
    {
        await Collection.DeleteOneAsync(x => x.Id == id);
    }
}