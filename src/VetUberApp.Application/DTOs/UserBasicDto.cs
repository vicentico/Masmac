namespace VetUberApp.Application.DTOs;

/// <summary>
/// DTO con información básica de un usuario
/// </summary>
public class UserBasicDto
{
    /// <summary>Identificador único del usuario</summary>
    public string Id { get; set; } = default!;
    
    /// <summary>Nombre completo del usuario</summary>
    public string Name { get; set; } = default!;
    
    /// <summary>Email del usuario</summary>
    public string Email { get; set; } = default!;
    
    /// <summary>Número de teléfono</summary>
    public string Phone { get; set; } = default!;
}