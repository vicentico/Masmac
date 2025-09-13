using System.Security.Cryptography;
using System.Text;
using VetUberApp.Application.DTOs;
using VetUberApp.Application.Interfaces;
using VetUberApp.Domain.Entities;
using VetUberApp.Domain.Interfaces;

namespace VetUberApp.Application.Services;

public class VeterinarianService : IVeterinarianService
{
    private readonly IVeterinarianRepository _veterinarianRepository;

    public VeterinarianService(IVeterinarianRepository veterinarianRepository)
    {
        _veterinarianRepository = veterinarianRepository;
    }

    public async Task<VeterinarianDto> CreateAsync(CreateVeterinarianDto dto)
    {
        if (await _veterinarianRepository.ExistsAsync(dto.Email))
        {
            throw new InvalidOperationException("Email already exists");
        }

        var veterinarian = new Veterinarian
        {
            Email = dto.Email,
            PasswordHash = HashPassword(dto.Password),
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            PhoneNumber = dto.PhoneNumber,
            LicenseNumber = dto.LicenseNumber,
            Specialties = dto.Specialties,
            Biography = dto.Biography,
            IsVerified = false,
            IsAvailable = true,
            CurrentLocation = dto.CurrentLocation,
            Rating = 0,
            ConsultationFee = dto.ConsultationFee,
            Reviews = new List<Review>(),
            Appointments = new List<Appointment>()
        };

        await _veterinarianRepository.CreateAsync(veterinarian);

        return MapToDto(veterinarian);
    }

    public async Task<VeterinarianDto?> GetByIdAsync(string id)
    {
        var veterinarian = await _veterinarianRepository.GetByIdAsync(id);
        return veterinarian != null ? MapToDto(veterinarian) : null;
    }

    public async Task<IEnumerable<VeterinarianDto>> GetAllAsync()
    {
        var veterinarians = await _veterinarianRepository.GetAllAsync();
        return veterinarians.Select(MapToDto);
    }

    public async Task<IEnumerable<VeterinarianDto>> GetAvailableAsync()
    {
        var veterinarians = await _veterinarianRepository.GetAvailableAsync();
        return veterinarians.Select(MapToDto);
    }

    public async Task<IEnumerable<VeterinarianDto>> FindBySpecialtyAsync(string specialty)
    {
        var veterinarians = await _veterinarianRepository.FindBySpecialtyAsync(specialty);
        return veterinarians.Select(MapToDto);
    }

    public async Task<VeterinarianDto> UpdateAsync(string id, UpdateVeterinarianDto dto)
    {
        var veterinarian = await _veterinarianRepository.GetByIdAsync(id)
            ?? throw new InvalidOperationException("Veterinarian not found");

        veterinarian.FirstName = dto.FirstName;
        veterinarian.LastName = dto.LastName;
        veterinarian.PhoneNumber = dto.PhoneNumber;
        veterinarian.ProfilePictureUrl = dto.ProfilePictureUrl;
        veterinarian.Specialties = dto.Specialties;
        veterinarian.Biography = dto.Biography;
        veterinarian.IsAvailable = dto.IsAvailable;
        veterinarian.CurrentLocation = dto.CurrentLocation;
        veterinarian.ConsultationFee = dto.ConsultationFee;

        await _veterinarianRepository.UpdateAsync(veterinarian);

        return MapToDto(veterinarian);
    }

    public async Task UpdateLocationAsync(string id, double latitude, double longitude)
    {
        await _veterinarianRepository.UpdateLocationAsync(id, latitude, longitude);
    }

    public async Task UpdateAvailabilityAsync(string id, bool isAvailable)
    {
        await _veterinarianRepository.UpdateAvailabilityAsync(id, isAvailable);
    }

    public async Task DeleteAsync(string id)
    {
        await _veterinarianRepository.DeleteAsync(id);
    }

    private static VeterinarianDto MapToDto(Veterinarian veterinarian)
    {
        return new VeterinarianDto(
            veterinarian.Id,
            veterinarian.Email,
            veterinarian.FirstName,
            veterinarian.LastName,
            veterinarian.PhoneNumber,
            veterinarian.ProfilePictureUrl,
            veterinarian.LicenseNumber,
            veterinarian.Specialties,
            veterinarian.Biography,
            veterinarian.IsVerified,
            veterinarian.IsAvailable,
            veterinarian.CurrentLocation,
            veterinarian.Rating,
            veterinarian.ConsultationFee
        );
    }

    private static string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(hashedBytes);
    }
}