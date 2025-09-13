using VetUberApp.Application.DTOs;
using VetUberApp.Domain.Entities;
using VetUberApp.Domain.ValueObjects;

namespace VetUberApp.Application.Extensions;

public static class MappingExtensions
{
    public static AddressDto ToDto(this Address address)
    {
        if (address == null)
            return null!;

        return new AddressDto
        {
            Street = address.Street,
            City = address.City,
            State = address.State,
            Country = address.Country,
            ZipCode = address.ZipCode,
            AdditionalInfo = address.AdditionalInfo
        };
    }

    public static Address ToEntity(this AddressDto dto)
    {
        if (dto == null)
            return null!;

        return new Address
        {
            Street = dto.Street,
            City = dto.City,
            State = dto.State,
            Country = dto.Country,
            ZipCode = dto.ZipCode,
            AdditionalInfo = dto.AdditionalInfo
        };
    }

    public static UserBasicDto ToBasicDto(this User user)
    {
        if (user == null)
            return null!;

        return new UserBasicDto
        {
            Id = user.Id,
            Name = user.FullName,
            Email = user.Email,
            Phone = user.PhoneNumber
        };
    }

    public static VeterinarianBasicDto ToBasicDto(this Veterinarian vet)
    {
        if (vet == null)
            return null!;

        return new VeterinarianBasicDto
        {
            Id = vet.Id,
            Name = vet.FullName,
            Specialty = vet.Specialization,
            LicenseNumber = vet.LicenseNumber
        };
    }

    public static PetBasicDto ToBasicDto(this Pet pet)
    {
        if (pet == null)
            return null!;

        return new PetBasicDto
        {
            Id = pet.Id,
            Name = pet.Name,
            Type = pet.Type,
            Breed = pet.Breed ?? "Desconocido",
            PhotoUrl = pet.PhotoUrl
        };
    }

    public static AppointmentBasicDto ToBasicDto(this Appointment appointment)
    {
        if (appointment == null)
            return null!;

        return new AppointmentBasicDto
        {
            Id = appointment.Id,
            VeterinarianName = appointment.Veterinarian?.FullName ?? "Desconocido",
            PetName = appointment.Pet?.Name ?? "Desconocido",
            ScheduledDateTime = appointment.ScheduledDateTime,
            Status = appointment.Status
        };
    }
}