using VetUberApp.Application.DTOs;

namespace VetUberApp.Application.Interfaces;

public interface IPetService
{
    Task<PetDto> CreateAsync(CreatePetDto dto);
    Task<PetDto?> GetByIdAsync(string id);
    Task<IEnumerable<PetDto>> GetAllAsync();
    Task<IEnumerable<PetDto>> GetByUserIdAsync(string userId);
    Task<PetDto> UpdateAsync(string id, UpdatePetDto dto);
    Task DeleteAsync(string id);
}