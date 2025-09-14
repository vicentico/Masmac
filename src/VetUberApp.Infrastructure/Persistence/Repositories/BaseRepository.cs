using System.Linq.Expressions;
using MongoDB.Bson;
using MongoDB.Driver;
using VetUberApp.Domain.Common;
using VetUberApp.Domain.Constants;
using VetUberApp.Infrastructure.Common;

namespace VetUberApp.Infrastructure.Persistence.Repositories;

/// <summary>
/// Repositorio base que proporciona operaciones CRUD comunes para todas las entidades
/// </summary>
/// <typeparam name="T">Tipo de entidad que hereda de BaseEntity</typeparam>
public abstract class BaseRepository<T> where T : BaseEntity
{
    protected readonly IMongoCollection<T> Collection;

    protected BaseRepository(MongoDbContext context, string collectionName)
    {
        Collection = context.GetCollection<T>(collectionName) 
            ?? throw new ArgumentNullException(nameof(context), "No se pudo obtener la colección de MongoDB");
    }

    /// <summary>
    /// Crea una nueva entidad en la base de datos
    /// </summary>
    /// <param name="entity">Entidad a crear</param>
    /// <returns>La entidad creada con su ID asignado</returns>
    /// <exception cref="ArgumentNullException">Si la entidad es null</exception>
    public virtual async Task<T> CreateAsync(T entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        
        if (string.IsNullOrEmpty(entity.Id))
        {
            entity.Id = ObjectId.GenerateNewId().ToString();
        }
        
        entity.CreatedAt = DateTime.UtcNow;
        await Collection.InsertOneAsync(entity);
        return entity;
    }

    /// <summary>
    /// Obtiene una entidad por su ID
    /// </summary>
    /// <param name="id">ID de la entidad</param>
    /// <returns>La entidad encontrada o null si no existe</returns>
    /// <exception cref="ArgumentException">Si el ID es null o vacío o no es un ObjectId válido</exception>
    public virtual async Task<T?> GetByIdAsync(string id)
    {
        MongoDbHelper.ValidateObjectId(id, nameof(id));

        return await Collection.Find(x => x.Id == id && !x.IsDeleted).FirstOrDefaultAsync();
    }

    /// <summary>
    /// Obtiene todas las entidades no eliminadas
    /// </summary>
    /// <returns>Lista de entidades</returns>
    public virtual async Task<IEnumerable<T>> GetAllAsync()
    {
        return await Collection.Find(x => !x.IsDeleted).ToListAsync();
    }

    /// <summary>
    /// Busca entidades que cumplan con el predicado especificado
    /// </summary>
    /// <param name="predicate">Expresión de filtro</param>
    /// <returns>Lista de entidades que cumplen el criterio</returns>
    /// <exception cref="ArgumentNullException">Si el predicado es null</exception>
    public virtual async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
    {
        ArgumentNullException.ThrowIfNull(predicate);
        
        var combinedPredicate = Expression.Lambda<Func<T, bool>>(
            Expression.AndAlso(
                predicate.Body,
                Expression.Equal(
                    Expression.Property(predicate.Parameters[0], nameof(BaseEntity.IsDeleted)),
                    Expression.Constant(false)
                )
            ),
            predicate.Parameters
        );
        
        return await Collection.Find(combinedPredicate).ToListAsync();
    }

    /// <summary>
    /// Actualiza una entidad existente
    /// </summary>
    /// <param name="entity">Entidad con los nuevos datos</param>
    /// <returns>La entidad actualizada</returns>
    /// <exception cref="ArgumentNullException">Si la entidad es null</exception>
    /// <exception cref="ArgumentException">Si el ID de la entidad no es un ObjectId válido</exception>
    /// <exception cref="InvalidOperationException">Si la entidad no existe</exception>
    public virtual async Task<T> UpdateAsync(T entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        MongoDbHelper.ValidateObjectId(entity.Id, nameof(entity.Id));
        
        entity.UpdatedAt = DateTime.UtcNow;
        
        var result = await Collection.ReplaceOneAsync(
            x => x.Id == entity.Id && !x.IsDeleted, 
            entity
        );
        
        if (result.MatchedCount == 0)
            throw new InvalidOperationException($"No se encontró la entidad con ID: {entity.Id}");
            
        return entity;
    }

    /// <summary>
    /// Elimina lógicamente una entidad (soft delete)
    /// </summary>
    /// <param name="id">ID de la entidad a eliminar</param>
    /// <returns>True si se eliminó correctamente, false si no se encontró la entidad</returns>
    /// <exception cref="ArgumentException">Si el ID es null, vacío o no es un ObjectId válido</exception>
    public virtual async Task<bool> DeleteAsync(string id)
    {
        MongoDbHelper.ValidateObjectId(id, nameof(id));

        var result = await Collection.UpdateOneAsync(
            x => x.Id == id && !x.IsDeleted,
            Builders<T>.Update
                .Set(x => x.IsDeleted, true)
                .Set(x => x.UpdatedAt, DateTime.UtcNow)
        );
        
        return result.MatchedCount > 0;
    }

    /// <summary>
    /// Elimina físicamente una entidad de la base de datos
    /// </summary>
    /// <param name="id">ID de la entidad a eliminar</param>
    /// <returns>True si se eliminó correctamente, false si no se encontró la entidad</returns>
    /// <exception cref="ArgumentException">Si el ID es null, vacío o no es un ObjectId válido</exception>
    public virtual async Task<bool> HardDeleteAsync(string id)
    {
        MongoDbHelper.ValidateObjectId(id, nameof(id));

        var result = await Collection.DeleteOneAsync(x => x.Id == id);
        
        return result.DeletedCount > 0;
    }
}