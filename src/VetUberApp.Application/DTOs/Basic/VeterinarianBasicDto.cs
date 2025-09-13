namespace VetUberApp.Application.DTOs.Basic;

/// <summary>
/// DTO para representar datos básicos de un veterinario
/// </summary>
public record VeterinarianBasicDto
{
    /// <summary>ID del veterinario</summary>
    public string Id { get; init; } = null!;
    /// <summary>Nombre completo</summary>
    public string Name { get; init; } = null!;
    /// <summary>Especialidad principal</summary>
    public string Specialty { get; init; } = null!;
    /// <summary>Número de licencia</summary>
    public string LicenseNumber { get; init; } = null!;
}