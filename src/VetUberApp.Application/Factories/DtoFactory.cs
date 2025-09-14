using VetUberApp.Application.DTOs;
using VetUberApp.Domain.Entities;

namespace VetUberApp.Application.Factories;

/// <summary>
/// Factory para crear DTOs a partir de entidades del dominio
/// </summary>
public static class DtoFactory
{
    /// <summary>
    /// Convierte una entidad Review a ReviewDto con entidades relacionadas
    /// </summary>
    /// <param name="review">Entidad Review</param>
    /// <param name="appointment">Entidad Appointment relacionada</param>
    /// <param name="user">Entidad User relacionada</param>
    /// <param name="veterinarian">Entidad Veterinarian relacionada</param>
    /// <returns>ReviewDto</returns>
    /// <exception cref="ArgumentNullException">Si algún parámetro es null</exception>
    public static ReviewDto CreateReviewDto(Review review, Appointment appointment, User user, Veterinarian veterinarian)
    {
        ArgumentNullException.ThrowIfNull(review);
        ArgumentNullException.ThrowIfNull(appointment);
        ArgumentNullException.ThrowIfNull(user);
        ArgumentNullException.ThrowIfNull(veterinarian);

        return new ReviewDto(
            Id: review.Id,
            Appointment: CreateAppointmentBasicDto(appointment),
            User: CreateUserBasicDto(user),
            Veterinarian: CreateVeterinarianBasicDto(veterinarian),
            Rating: review.Rating,
            Comment: review.Comment,
            Type: review.Type,
            CreatedAt: review.CreatedAt,
            UpdatedAt: review.UpdatedAt
        );
    }

    /// <summary>
    /// Convierte una entidad User a UserBasicDto
    /// </summary>
    /// <param name="user">Entidad User</param>
    /// <returns>UserBasicDto</returns>
    /// <exception cref="ArgumentNullException">Si user es null</exception>
    public static UserBasicDto CreateUserBasicDto(User user)
    {
        ArgumentNullException.ThrowIfNull(user);

        return new UserBasicDto
        {
            Id = user.Id,
            Name = user.FullName,
            Email = user.Email,
            Phone = user.PhoneNumber
        };
    }

    /// <summary>
    /// Convierte una entidad Veterinarian a VeterinarianBasicDto
    /// </summary>
    /// <param name="veterinarian">Entidad Veterinarian</param>
    /// <returns>VeterinarianBasicDto</returns>
    /// <exception cref="ArgumentNullException">Si veterinarian es null</exception>
    public static VeterinarianBasicDto CreateVeterinarianBasicDto(Veterinarian veterinarian)
    {
        ArgumentNullException.ThrowIfNull(veterinarian);

        return new VeterinarianBasicDto
        {
            Id = veterinarian.Id,
            Name = veterinarian.FullName,
            Specialty = veterinarian.Specialization,
            LicenseNumber = veterinarian.LicenseNumber,
            Rating = (double)veterinarian.Rating
        };
    }

    /// <summary>
    /// Convierte una entidad Pet a PetBasicDto
    /// </summary>
    /// <param name="pet">Entidad Pet</param>
    /// <returns>PetBasicDto</returns>
    /// <exception cref="ArgumentNullException">Si pet es null</exception>
    public static PetBasicDto CreatePetBasicDto(Pet pet)
    {
        ArgumentNullException.ThrowIfNull(pet);

        return new PetBasicDto
        {
            Id = pet.Id,
            Name = pet.Name,
            Type = pet.Type,
            Breed = pet.Breed,
            PhotoUrl = pet.PhotoUrl
        };
    }

    /// <summary>
    /// Convierte una entidad Appointment a AppointmentBasicDto
    /// </summary>
    /// <param name="appointment">Entidad Appointment</param>
    /// <returns>AppointmentBasicDto</returns>
    /// <exception cref="ArgumentNullException">Si appointment es null</exception>
    public static AppointmentBasicDto CreateAppointmentBasicDto(Appointment appointment)
    {
        ArgumentNullException.ThrowIfNull(appointment);

        return new AppointmentBasicDto
        {
            Id = appointment.Id,
            VeterinarianName = appointment.Veterinarian?.FullName ?? "No asignado",
            PetName = appointment.Pet?.Name ?? "No asignado",
            ScheduledDateTime = appointment.ScheduledDateTime,
            Status = appointment.Status
        };
    }
}