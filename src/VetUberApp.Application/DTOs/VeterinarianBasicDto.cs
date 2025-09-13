namespace VetUberApp.Application.DTOs;

/// <summary>
/// DTO con información básica de un veterinario
/// </summary>
public class VeterinarianBasicDto
{
    /// <summary>
    /// ID único del veterinario
    /// </summary>
    public string Id { get; set; } = default!;

    /// <summary>
    /// Nombre completo del veterinario
    /// </summary>
    public string Name { get; set; } = default!;

    /// <summary>
    /// Especialidades del veterinario (separadas por coma)
    /// </summary>
    public string Specialty { get; set; } = default!;

    /// <summary>
    /// Número de licencia profesional
    /// </summary>
    public string LicenseNumber { get; set; } = default!;

    /// <summary>
    /// Calificación promedio del veterinario
    /// </summary>
    public double Rating { get; set; }
}