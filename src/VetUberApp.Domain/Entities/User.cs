using VetUberApp.Domain.Common;
using VetUberApp.Domain.Enums;
using VetUberApp.Domain.ValueObjects;

namespace VetUberApp.Domain.Entities;

/// <summary>
/// Representa un usuario cliente del sistema
/// </summary>
public class User : BaseEntity
{
    #region Información Personal
    /// <summary>
    /// Dirección de correo electrónico del usuario
    /// </summary>
    public string Email { get; set; } = string.Empty;
    
    /// <summary>
    /// Hash de la contraseña del usuario
    /// </summary>
    public string PasswordHash { get; set; } = string.Empty;
    
    /// <summary>
    /// Nombre del usuario
    /// </summary>
    public string FirstName { get; set; } = string.Empty;
    
    /// <summary>
    /// Apellido del usuario
    /// </summary>
    public string LastName { get; set; } = string.Empty;
    
    /// <summary>
    /// Número de teléfono del usuario
    /// </summary>
    public string PhoneNumber { get; set; } = string.Empty;
    
    /// <summary>
    /// URL de la foto de perfil (opcional)
    /// </summary>
    public string? ProfilePictureUrl { get; set; }
    #endregion

    #region Propiedades de Estado
    /// <summary>
    /// Rol del usuario en el sistema
    /// </summary>
    public UserRole Role { get; set; } = UserRole.Client;
    
    /// <summary>
    /// Indica si el usuario ha verificado su cuenta
    /// </summary>
    public bool IsVerified { get; set; } = false;
    #endregion

    #region Ubicación
    /// <summary>
    /// Dirección principal del usuario
    /// </summary>
    public Address Address { get; set; } = default!;
    #endregion

    #region Propiedades Calculadas
    /// <summary>
    /// Nombre completo del usuario
    /// </summary>
    public string FullName => $"{FirstName} {LastName}".Trim();
    #endregion

    #region Relaciones
    /// <summary>
    /// Lista de mascotas del usuario
    /// </summary>
    public List<Pet> Pets { get; set; } = new();
    
    /// <summary>
    /// Lista de reseñas dadas por el usuario
    /// </summary>
    public List<Review> ReviewsGiven { get; set; } = new();
    
    /// <summary>
    /// Lista de citas del usuario
    /// </summary>
    public List<Appointment> Appointments { get; set; } = new();
    #endregion
}