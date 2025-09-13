using VetUberApp.Domain.Enums;
using VetUberApp.Domain.ValueObjects;

namespace VetUberApp.Application.DTOs;

/// <summary>
/// DTO para crear una nueva cita
/// </summary>
public class CreateAppointmentDto
{
    /// <summary>
    /// ID del veterinario asignado a la cita
    /// </summary>
    public string VeterinarianId { get; set; } = null!;

    /// <summary>
    /// ID del dueño de la mascota
    /// </summary>
    public string OwnerId { get; set; } = null!;

    /// <summary>
    /// ID de la mascota
    /// </summary>
    public string PetId { get; set; } = null!;

    /// <summary>
    /// Fecha y hora programada para la cita
    /// </summary>
    public DateTime ScheduledDateTime { get; set; }

    /// <summary>
    /// Motivo de la consulta
    /// </summary>
    public string Reason { get; set; } = null!;

    /// <summary>
    /// Tipo de cita (consulta regular, emergencia, etc.)
    /// </summary>
    public AppointmentType Type { get; set; }

    /// <summary>
    /// Dirección donde se realizará la cita (puede ser null si es en clínica)
    /// </summary>
    public AddressDto? Location { get; set; }
}

/// <summary>
/// DTO para actualizar una cita existente
/// </summary>
public class UpdateAppointmentDto
{
    /// <summary>
    /// Nueva fecha y hora programada para la cita
    /// </summary>
    public DateTime? ScheduledDateTime { get; set; }

    /// <summary>
    /// Nuevo motivo de la consulta
    /// </summary>
    public string? Reason { get; set; }

    /// <summary>
    /// Nuevo tipo de cita
    /// </summary>
    public AppointmentType? Type { get; set; }

    /// <summary>
    /// Nueva dirección donde se realizará la cita
    /// </summary>
    public AddressDto? Location { get; set; }

    /// <summary>
    /// Estado actualizado de la cita
    /// </summary>
    public AppointmentStatus? Status { get; set; }
}

/// <summary>
/// DTO para representar una cita completa
/// </summary>
public class AppointmentDto
{
    /// <summary>
    /// ID único de la cita
    /// </summary>
    public string Id { get; set; } = null!;

    /// <summary>
    /// Información del veterinario asignado
    /// </summary>
    public VeterinarianBasicDto Veterinarian { get; set; } = null!;

    /// <summary>
    /// Información del dueño de la mascota
    /// </summary>
    public UserBasicDto Owner { get; set; } = null!;

    /// <summary>
    /// Información de la mascota
    /// </summary>
    public PetBasicDto Pet { get; set; } = null!;

    /// <summary>
    /// Fecha y hora programada
    /// </summary>
    public DateTime ScheduledDateTime { get; set; }

    /// <summary>
    /// Motivo de la consulta
    /// </summary>
    public string Reason { get; set; } = null!;

    /// <summary>
    /// Tipo de cita
    /// </summary>
    public AppointmentType Type { get; set; }

    /// <summary>
    /// Estado actual de la cita
    /// </summary>
    public AppointmentStatus Status { get; set; }

    /// <summary>
    /// Dirección donde se realizará la cita
    /// </summary>
    public AddressDto? Location { get; set; }

    /// <summary>
    /// Fecha de creación de la cita
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Última fecha de actualización de la cita
    /// </summary>
    public DateTime? UpdatedAt { get; set; }
}