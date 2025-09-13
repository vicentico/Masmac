using VetUberApp.Domain.Entities;

namespace VetUberApp.Domain.Interfaces;

/// <summary>
/// Interfaz para el repositorio de citas
/// </summary>
public interface IAppointmentRepository : IBaseRepository<Appointment>
{
    /// <summary>
    /// Verifica si un veterinario está disponible en una fecha y hora específica
    /// </summary>
    /// <param name="veterinarianId">ID del veterinario</param>
    /// <param name="dateTime">Fecha y hora a verificar</param>
    /// <returns>True si el veterinario está disponible, False en caso contrario</returns>
    Task<bool> IsVeterinarianAvailableAsync(string veterinarianId, DateTime dateTime);

    /// <summary>
    /// Obtiene todas las citas de un veterinario específico
    /// </summary>
    /// <param name="veterinarianId">ID del veterinario</param>
    /// <returns>Lista de citas del veterinario</returns>
    Task<IEnumerable<Appointment>> GetByVeterinarianIdAsync(string veterinarianId);

    /// <summary>
    /// Obtiene todas las citas de un dueño específico
    /// </summary>
    /// <param name="ownerId">ID del dueño de la mascota</param>
    /// <returns>Lista de citas del dueño</returns>
    Task<IEnumerable<Appointment>> GetByOwnerIdAsync(string ownerId);

    /// <summary>
    /// Obtiene todas las citas de una mascota específica
    /// </summary>
    /// <param name="petId">ID de la mascota</param>
    /// <returns>Lista de citas de la mascota</returns>
    Task<IEnumerable<Appointment>> GetByPetIdAsync(string petId);
}