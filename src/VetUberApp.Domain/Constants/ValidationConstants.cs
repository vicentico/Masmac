namespace VetUberApp.Domain.Constants;

/// <summary>
/// Constantes para validaciones del sistema
/// </summary>
public static class ValidationConstants
{
    /// <summary>
    /// Longitud estándar para ObjectId de MongoDB
    /// </summary>
    public const int MongoObjectIdLength = 24;

    /// <summary>
    /// Calificación mínima permitida
    /// </summary>
    public const decimal MinRating = 1.00m;

    /// <summary>
    /// Calificación máxima permitida
    /// </summary>
    public const decimal MaxRating = 5.00m;

    /// <summary>
    /// Número máximo de decimales para calificaciones
    /// </summary>
    public const int RatingDecimalPlaces = 2;

    /// <summary>
    /// Longitud mínima para comentarios
    /// </summary>
    public const int MinCommentLength = 10;

    /// <summary>
    /// Longitud máxima para comentarios
    /// </summary>
    public const int MaxCommentLength = 500;
}

/// <summary>
/// Mensajes de error estandarizados
/// </summary>
public static class ErrorMessages
{
    public const string RequiredField = "Este campo es requerido";
    public const string InvalidObjectId = "El ID debe tener 24 caracteres (formato MongoDB)";
    public const string InvalidRatingRange = "La calificación debe estar entre 1.00 y 5.00";
    public const string InvalidRatingDecimals = "La calificación debe tener máximo 2 decimales";
    public const string InvalidCommentLength = "El comentario debe tener entre 10 y 500 caracteres";
    
    public static class Entities
    {
        public const string AppointmentNotFound = "La cita especificada no existe";
        public const string UserNotFound = "El usuario especificado no existe";
        public const string VeterinarianNotFound = "El veterinario especificado no existe";
        public const string ReviewNotFound = "La reseña especificada no existe";
        public const string DuplicateReview = "Ya existe una reseña para esta cita";
    }
}