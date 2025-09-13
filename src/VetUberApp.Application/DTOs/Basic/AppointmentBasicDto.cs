using VetUberApp.Domain.Enums;

namespace VetUberApp.Application.DTOs.Basic;

/// <summary>
/// DTO para representar datos básicos de una cita
/// </summary>
public record AppointmentBasicDto
{
    /// <summary>ID de la cita</summary>
    public string Id { get; init; } = null!;
    /// <summary>Nombre completo del veterinario</summary>
    public string VeterinarianName { get; init; } = null!;
    /// <summary>Nombre de la mascota</summary>
    public string PetName { get; init; } = null!;
    /// <summary>Fecha y hora programada</summary>
    public DateTime ScheduledDateTime { get; init; }
    /// <summary>Estado de la cita</summary>
    public AppointmentStatus Status { get; init; }
}