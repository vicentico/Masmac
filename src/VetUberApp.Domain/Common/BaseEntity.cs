namespace VetUberApp.Domain.Common;

/// <summary>
/// Clase base para todas las entidades del dominio
/// </summary>
public abstract class BaseEntity
{
    /// <summary>
    /// Identificador único de la entidad
    /// </summary>
    public virtual string Id { get; set; } = string.Empty;
    
    /// <summary>
    /// Fecha y hora de creación de la entidad en UTC
    /// </summary>
    public virtual DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    /// <summary>
    /// Fecha y hora de última actualización en UTC
    /// </summary>
    public virtual DateTime? UpdatedAt { get; set; }
    
    /// <summary>
    /// Indica si la entidad ha sido marcada como eliminada (soft delete)
    /// </summary>
    public virtual bool IsDeleted { get; set; } = false;
}