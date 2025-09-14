namespace VetUberApp.Domain.Constants;

/// <summary>
/// Constantes para códigos de error y mensajes del sistema
/// </summary>
/// <remarks>
/// Centraliza todos los mensajes de error y códigos de respuesta
/// para mejorar la mantenibilidad y consistencia.
/// </remarks>
public static class ErrorConstants
{
    #region Review Errors

    /// <summary>
    /// Mensajes de error relacionados con reseñas
    /// </summary>
    public static class Reviews
    {
        public const string NotFound = "No se encontró la reseña especificada.";
        public const string InvalidId = "La reseña no tiene un ID válido.";
        public const string UserAlreadyReviewed = "El usuario ya ha creado una reseña para esta cita.";
        public const string CannotDelete = "No se pudo eliminar la reseña.";
    }

    #endregion

    #region Appointment Errors

    /// <summary>
    /// Mensajes de error relacionados con citas
    /// </summary>
    public static class Appointments
    {
        public const string NotFound = "La cita especificada no existe.";
        public const string NotFoundForReview = "No se encontró la cita asociada a la reseña.";
        public const string InvalidStatus = "El estado de la cita no es válido.";
    }

    #endregion

    #region User Errors

    /// <summary>
    /// Mensajes de error relacionados con usuarios
    /// </summary>
    public static class Users
    {
        public const string NotFound = "No se encontró el usuario especificado.";
        public const string NotFoundForReview = "No se encontró el usuario asociado a la reseña.";
        public const string InvalidEmail = "El formato del email no es válido.";
    }

    #endregion

    #region Veterinarian Errors

    /// <summary>
    /// Mensajes de error relacionados con veterinarios
    /// </summary>
    public static class Veterinarians
    {
        public const string NotFound = "No se encontró el veterinario especificado.";
        public const string NotFoundForReview = "No se encontró el veterinario asociado a la reseña.";
        public const string InvalidLicense = "El número de licencia no es válido.";
    }

    #endregion

    #region Pet Errors

    /// <summary>
    /// Mensajes de error relacionados con mascotas
    /// </summary>
    public static class Pets
    {
        public const string NotFound = "No se encontró la mascota especificada.";
        public const string NotFoundForAppointment = "No se encontró la mascota asociada a la cita.";
        public const string InvalidType = "El tipo de mascota no es válido.";
    }

    #endregion

    #region Repository Errors

    /// <summary>
    /// Mensajes de error relacionados con operaciones de repositorio
    /// </summary>
    public static class Repository
    {
        public const string InvalidId = "El ID no puede ser null o vacío";
        public const string CreateFailed = "No se pudo crear la entidad.";
        public const string UpdateFailed = "No se pudo actualizar la entidad.";
        public const string DeleteFailed = "No se pudo eliminar la entidad.";
    }

    #endregion

    #region Validation Errors

    /// <summary>
    /// Mensajes de error relacionados con validación
    /// </summary>
    public static class Validation
    {
        public const string Required = "Este campo es requerido.";
        public const string InvalidFormat = "El formato no es válido.";
        public const string OutOfRange = "El valor está fuera del rango permitido.";
        public const string TooLong = "El valor excede la longitud máxima permitida.";
        public const string TooShort = "El valor no cumple con la longitud mínima requerida.";
    }

    #endregion

    #region HTTP Status Messages

    /// <summary>
    /// Mensajes estándar para respuestas HTTP
    /// </summary>
    public static class Http
    {
        public const string ValidationError = "Error de validación";
        public const string InternalServerError = "Error interno del servidor";
        public const string NotFound = "Recurso no encontrado";
        public const string Unauthorized = "No autorizado";
        public const string BadRequest = "Solicitud incorrecta";
    }

    #endregion
}