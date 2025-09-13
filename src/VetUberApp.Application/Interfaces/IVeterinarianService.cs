using VetUberApp.Application.DTOs;

namespace VetUberApp.Application.Interfaces;

public interface IVeterinarianService
{
    Task<VeterinarianDto> CreateAsync(CreateVeterinarianDto dto);
    Task<VeterinarianDto?> GetByIdAsync(string id);
    Task<IEnumerable<VeterinarianDto>> GetAllAsync();
    Task<VeterinarianDto> UpdateAsync(string id, UpdateVeterinarianDto dto);
    Task DeleteAsync(string id);
}