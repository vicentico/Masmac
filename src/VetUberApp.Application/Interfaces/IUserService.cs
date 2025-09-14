using VetUberApp.Application.DTOs;

namespace VetUberApp.Application.Interfaces;

public interface IUserService
{
    Task<UserDto> CreateAsync(CreateUserDto dto);
    Task<UserDto?> GetByIdAsync(string id);
    Task<IEnumerable<UserDto>> GetAllAsync();
    Task<UserDto> UpdateAsync(string id, UpdateUserDto dto);
    Task<bool> DeleteAsync(string id);
}