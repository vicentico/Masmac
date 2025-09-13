using VetUberApp.Domain.ValueObjects;

namespace VetUberApp.Application.DTOs;

/// <summary>
/// DTO para crear un nuevo veterinario
/// </summary>
public record CreateVeterinarianDto(
    /// <summary>Email del veterinario, debe ser único</summary>
    string Email,
    /// <summary>Contraseña del veterinario</summary>
    string Password,
    /// <summary>Nombre del veterinario</summary>
    string FirstName,
    /// <summary>Apellido del veterinario</summary>
    string LastName,
    /// <summary>Número de teléfono</summary>
    string PhoneNumber,
    /// <summary>Número de licencia profesional</summary>
    string LicenseNumber,
    /// <summary>Lista de especialidades</summary>
    List<string> Specialties,
    /// <summary>Biografía o descripción profesional</summary>
    string? Biography,
    /// <summary>Ubicación actual del veterinario</summary>
    Address CurrentLocation,
    /// <summary>Tarifa de consulta</summary>
    decimal ConsultationFee
);

/// <summary>
/// DTO para actualizar un veterinario existente
/// </summary>
public record UpdateVeterinarianDto(
    /// <summary>Nombre del veterinario</summary>
    string FirstName,
    /// <summary>Apellido del veterinario</summary>
    string LastName,
    /// <summary>Número de teléfono</summary>
    string PhoneNumber,
    /// <summary>URL de la foto de perfil</summary>
    string? ProfilePictureUrl,
    /// <summary>Lista de especialidades</summary>
    List<string> Specialties,
    /// <summary>Biografía o descripción profesional</summary>
    string? Biography,
    /// <summary>Ubicación actual del veterinario</summary>
    Address CurrentLocation,
    /// <summary>Indica si está disponible para consultas</summary>
    bool IsAvailable,
    /// <summary>Tarifa de consulta</summary>
    decimal ConsultationFee
);

/// <summary>
/// DTO con los datos completos de un veterinario
/// </summary>
public record VeterinarianDto(
    /// <summary>Identificador único del veterinario</summary>
    string Id,
    /// <summary>Email del veterinario</summary>
    string Email,
    /// <summary>Nombre del veterinario</summary>
    string FirstName,
    /// <summary>Apellido del veterinario</summary>
    string LastName,
    /// <summary>Número de teléfono</summary>
    string PhoneNumber,
    /// <summary>URL de la foto de perfil</summary>
    string? ProfilePictureUrl,
    /// <summary>Número de licencia profesional</summary>
    string LicenseNumber,
    /// <summary>Lista de especialidades</summary>
    List<string> Specialties,
    /// <summary>Biografía o descripción profesional</summary>
    string? Biography,
    /// <summary>Indica si está verificado</summary>
    bool IsVerified,
    /// <summary>Indica si está disponible para consultas</summary>
    bool IsAvailable,
    /// <summary>Ubicación actual del veterinario</summary>
    Address CurrentLocation,
    /// <summary>Calificación promedio</summary>
    decimal Rating,
    /// <summary>Tarifa de consulta</summary>
    decimal ConsultationFee
);