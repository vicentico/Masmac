using VetUberApp.Domain.Entities;
using VetUberApp.Domain.Enums;
using VetUberApp.Domain.ValueObjects;

namespace VetUberApp.Application.DTOs;

/// <summary>
/// DTO para crear un nuevo usuario
/// </summary>
public record CreateUserDto(
    /// <summary>Email del usuario, debe ser único</summary>
    string Email,
    /// <summary>Contraseña del usuario</summary>
    string Password,
    /// <summary>Nombre del usuario</summary>
    string FirstName,
    /// <summary>Apellido del usuario</summary>
    string LastName,
    /// <summary>Número de teléfono del usuario</summary>
    string PhoneNumber,
    /// <summary>Dirección del usuario</summary>
    Address Address
);

/// <summary>
/// DTO para actualizar un usuario existente
/// </summary>
public record UpdateUserDto(
    /// <summary>Nuevo nombre del usuario</summary>
    string FirstName,
    /// <summary>Nuevo apellido del usuario</summary>
    string LastName,
    /// <summary>Nuevo número de teléfono</summary>
    string PhoneNumber,
    /// <summary>Nueva URL de la foto de perfil (opcional)</summary>
    string? ProfilePictureUrl,
    /// <summary>Nueva dirección del usuario</summary>
    Address Address
);

/// <summary>
/// DTO con los datos completos de un usuario
/// </summary>
public record UserDto(
    /// <summary>Identificador único del usuario</summary>
    string Id,
    /// <summary>Email del usuario</summary>
    string Email,
    /// <summary>Nombre del usuario</summary>
    string FirstName,
    /// <summary>Apellido del usuario</summary>
    string LastName,
    /// <summary>Número de teléfono</summary>
    string PhoneNumber,
    /// <summary>URL de la foto de perfil (opcional)</summary>
    string? ProfilePictureUrl,
    /// <summary>Rol del usuario en el sistema</summary>
    UserRole Role,
    /// <summary>Indica si el usuario está verificado</summary>
    bool IsVerified,
    /// <summary>Dirección del usuario</summary>
    Address Address
);