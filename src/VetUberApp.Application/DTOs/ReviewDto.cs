using System.ComponentModel.DataAnnotations;
using VetUberApp.Domain.Enums;
using VetUberApp.Domain.ValueObjects;
using VetUberApp.Application.DTOs.Basic;

namespace VetUberApp.Application.DTOs;

/// <summary>
/// DTO con los datos completos de una reseña
/// </summary>
public record ReviewDto(
    /// <summary>ID de la reseña</summary>
    string Id,
    /// <summary>Información básica de la cita</summary>
    AppointmentBasicDto Appointment,
    /// <summary>Información básica del usuario que creó la reseña</summary>
    UserBasicDto User,
    /// <summary>Información básica del veterinario reseñado</summary>
    VeterinarianBasicDto Veterinarian,
    /// <summary>Calificación (1-5) con hasta dos decimales</summary>
    decimal Rating,
    /// <summary>Comentario de la reseña</summary>
    string Comment,
    /// <summary>Tipo de reseña</summary>
    ReviewType Type,
    /// <summary>Fecha de creación</summary>
    DateTime CreatedAt,
    /// <summary>Fecha de última actualización</summary>
    DateTime? UpdatedAt
);

/// <summary>
/// DTO para crear una nueva reseña
/// </summary>
public record CreateReviewDto(
    /// <summary>ID de la cita</summary>
    [Required(AllowEmptyStrings = false)]
    [StringLength(24, MinimumLength = 1)]
    string AppointmentId,
    /// <summary>ID del usuario que crea la reseña</summary>
    [Required] string UserId,
    /// <summary>ID del veterinario que se está reseñando</summary>
    [Required] string VeterinarianId,
    /// <summary>Calificación (1-5) con hasta dos decimales</summary>
    [Required]
    [Range(typeof(decimal), "1.00", "5.00", ErrorMessage = "La calificación debe estar entre 1.00 y 5.00")]
    decimal Rating,
    /// <summary>Comentario de la reseña</summary>
    [Required] string Comment,
    /// <summary>Tipo de reseña</summary>
    [Required] ReviewType Type
);

/// <summary>
/// DTO para actualizar una reseña existente
/// </summary>
public record UpdateReviewDto(
    /// <summary>Nueva calificación (1-5) con hasta dos decimales</summary>
    [Range(typeof(decimal), "1.00", "5.00", ErrorMessage = "La calificación debe estar entre 1.00 y 5.00")]
    decimal? Rating = null,
    /// <summary>Nuevo comentario</summary>
    string? Comment = null,
    /// <summary>Nuevo tipo de reseña</summary>
    ReviewType? Type = null
);