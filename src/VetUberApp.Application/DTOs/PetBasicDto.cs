using VetUberApp.Domain.Enums;

namespace VetUberApp.Application.DTOs;

public class PetBasicDto
{
    /// <summary>
    /// ID único de la mascota
    /// </summary>
    public string Id { get; set; } = default!;

    /// <summary>
    /// Nombre de la mascota
    /// </summary>
    public string Name { get; set; } = default!;

    /// <summary>
    /// Tipo de mascota (perro, gato, etc.)
    /// </summary>
    public PetType Type { get; set; }

    /// <summary>
    /// Raza de la mascota
    /// </summary>
    public string Breed { get; set; } = default!;

    /// <summary>
    /// URL de la foto de la mascota
    /// </summary>
    public string? PhotoUrl { get; set; }
}