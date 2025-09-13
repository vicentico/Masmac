using VetUberApp.Domain.Entities;

namespace VetUberApp.Domain.Interfaces;

/// <summary>
/// Interfaz para el repositorio de reseñas
/// </summary>
public interface IReviewRepository : IBaseRepository<Review>
{
    /// <summary>
    /// Obtiene todas las reseñas de un veterinario específico
    /// </summary>
    /// <param name="veterinarianId">ID del veterinario</param>
    /// <returns>Lista de reseñas del veterinario</returns>
    Task<IEnumerable<Review>> GetReviewsByVeterinarianId(string veterinarianId);

    /// <summary>
    /// Obtiene todas las reseñas realizadas por un usuario específico
    /// </summary>
    /// <param name="userId">ID del usuario</param>
    /// <returns>Lista de reseñas del usuario</returns>
    Task<IEnumerable<Review>> GetReviewsByUserId(string userId);

    /// <summary>
    /// Obtiene todas las reseñas de una cita específica
    /// </summary>
    /// <param name="appointmentId">ID de la cita</param>
    /// <returns>Lista de reseñas de la cita</returns>
    Task<IEnumerable<Review>> GetReviewsByAppointmentId(string appointmentId);

    /// <summary>
    /// Verifica si un usuario ha realizado una reseña para una cita específica
    /// </summary>
    /// <param name="userId">ID del usuario</param>
    /// <param name="appointmentId">ID de la cita</param>
    /// <returns>True si el usuario ya ha realizado una reseña, False en caso contrario</returns>
    Task<bool> HasUserReviewedAppointmentAsync(string userId, string appointmentId);

    /// <summary>
    /// Obtiene el promedio de calificaciones de un veterinario
    /// </summary>
    /// <param name="veterinarianId">ID del veterinario</param>
    /// <returns>Promedio de calificaciones con dos decimales</returns>
    Task<decimal> GetAverageRatingByVeterinarianIdAsync(string veterinarianId);
}