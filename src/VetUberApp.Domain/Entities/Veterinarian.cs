using VetUberApp.Domain.Common;
using VetUberApp.Domain.ValueObjects;

namespace VetUberApp.Domain.Entities;

/// <summary>
/// Representa un veterinario profesional en el sistema
/// </summary>
public class Veterinarian : BaseEntity
{
    #region Información Personal
    /// <summary>
    /// Dirección de correo electrónico del veterinario
    /// </summary>
    public string Email { get; set; } = string.Empty;
    
    /// <summary>
    /// Hash de la contraseña del veterinario
    /// </summary>
    public string PasswordHash { get; set; } = string.Empty;
    
    /// <summary>
    /// Nombre del veterinario
    /// </summary>
    public string FirstName { get; set; } = string.Empty;
    
    /// <summary>
    /// Apellido del veterinario
    /// </summary>
    public string LastName { get; set; } = string.Empty;
    
    /// <summary>
    /// Número de teléfono del veterinario
    /// </summary>
    public string PhoneNumber { get; set; } = string.Empty;
    
    /// <summary>
    /// URL de la foto de perfil (opcional)
    /// </summary>
    public string? ProfilePictureUrl { get; set; }
    
    /// <summary>
    /// Biografía profesional del veterinario (opcional)
    /// </summary>
    public string? Biography { get; set; }
    #endregion

    #region Información Profesional
    /// <summary>
    /// Número de licencia profesional
    /// </summary>
    public string LicenseNumber { get; set; } = string.Empty;
    
    /// <summary>
    /// Lista de especialidades del veterinario
    /// </summary>
    public List<string> Specialties { get; set; } = new();
    
    /// <summary>
    /// Tarifa de consulta del veterinario
    /// </summary>
    public decimal ConsultationFee { get; set; }
    #endregion

    #region Estado y Disponibilidad
    /// <summary>
    /// Indica si el veterinario ha sido verificado por el sistema
    /// </summary>
    public bool IsVerified { get; set; } = false;
    
    /// <summary>
    /// Indica si el veterinario está disponible para nuevas citas
    /// </summary>
    public bool IsAvailable { get; set; } = true;
    
    /// <summary>
    /// Ubicación actual del veterinario
    /// </summary>
    public Address CurrentLocation { get; set; } = default!;
    
    /// <summary>
    /// Calificación promedio del veterinario (0.00 - 5.00)
    /// </summary>
    public decimal Rating { get; set; } = 0.00m;
    #endregion

    #region Propiedades Calculadas
    /// <summary>
    /// Nombre completo del veterinario
    /// </summary>
    public string FullName => $"{FirstName} {LastName}".Trim();
    
    /// <summary>
    /// Especialización como texto concatenado
    /// </summary>
    public string Specialization => string.Join(", ", Specialties);
    #endregion

    #region Relaciones
    /// <summary>
    /// Lista de reseñas recibidas por el veterinario
    /// </summary>
    public List<Review> Reviews { get; set; } = new();
    
    /// <summary>
    /// Lista de citas del veterinario
    /// </summary>
    public List<Appointment> Appointments { get; set; } = new();
    #endregion
}