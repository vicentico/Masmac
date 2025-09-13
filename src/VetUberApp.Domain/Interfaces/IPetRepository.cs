using VetUberApp.Domain.Entities;

namespace VetUberApp.Domain.Interfaces;

public interface IPetRepository : IBaseRepository<Pet>
{
    Task<IEnumerable<Pet>> GetByOwnerIdAsync(string ownerId);
    Task<bool> BelongsToOwnerAsync(string petId, string ownerId);
    Task<IEnumerable<Pet>> FindByTypeAsync(string type);
}