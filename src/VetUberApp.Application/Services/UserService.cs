using System.Security.Cryptography;
using System.Text;
using VetUberApp.Application.DTOs; 
using VetUberApp.Domain.Entities;
using VetUberApp.Domain.Enums;
using VetUberApp.Domain.Interfaces;

namespace VetUberApp.Application.Services;

public class UserService : VetUberApp.Application.Interfaces.IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<UserDto> CreateAsync(CreateUserDto dto)
    {
        if (await _userRepository.ExistsAsync(dto.Email))
        {
            throw new InvalidOperationException("Email already exists");
        }

        var user = new User
        {
            Email = dto.Email,
            PasswordHash = HashPassword(dto.Password),
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            PhoneNumber = dto.PhoneNumber,
            Role = UserRole.Client,
            IsVerified = false,
            Address = dto.Address,
            Pets = new List<Pet>(),
            ReviewsGiven = new List<Review>(),
            Appointments = new List<Appointment>()
        };

        await _userRepository.CreateAsync(user);

        return MapToDto(user);
    }

    public async Task<UserDto?> GetByIdAsync(string id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        return user != null ? MapToDto(user) : null;
    }

    public async Task<IEnumerable<UserDto>> GetAllAsync()
    {
        var users = await _userRepository.GetAllAsync();
        return users.Select(MapToDto);
    }

    public async Task<UserDto> UpdateAsync(string id, UpdateUserDto dto)
    {
        var user = await _userRepository.GetByIdAsync(id) 
            ?? throw new InvalidOperationException("User not found");

        user.FirstName = dto.FirstName;
        user.LastName = dto.LastName;
        user.PhoneNumber = dto.PhoneNumber;
        user.ProfilePictureUrl = dto.ProfilePictureUrl;
        user.Address = dto.Address;

        await _userRepository.UpdateAsync(user);

        return MapToDto(user);
    }

    public async Task DeleteAsync(string id)
    {
        await _userRepository.DeleteAsync(id);
    }

    private static UserDto MapToDto(User user)
    {
        return new UserDto(
            user.Id,
            user.Email,
            user.FirstName,
            user.LastName,
            user.PhoneNumber,
            user.ProfilePictureUrl,
            user.Role,
            user.IsVerified,
            user.Address
        );
    }

    private static string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(hashedBytes);
    }
}