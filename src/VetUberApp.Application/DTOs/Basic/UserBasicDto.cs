namespace VetUberApp.Application.DTOs.Basic;

/// <summary>
/// DTO para representar datos básicos de un usuario
/// </summary>
public record UserBasicDto
{
    /// <summary>ID del usuario</summary>
    public string Id { get; init; } = null!;
    /// <summary>Nombre completo</summary>
    public string Name { get; init; } = null!;
    /// <summary>Correo electrónico</summary>
    public string Email { get; init; } = null!;
    /// <summary>Teléfono</summary>
    public string Phone { get; init; } = null!;
}