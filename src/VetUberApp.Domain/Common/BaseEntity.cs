namespace VetUberApp.Domain.Common;

public abstract class BaseEntity
{
    public virtual string Id { get; set; } = default!;
    public virtual DateTime CreatedAt { get; set; }
    public virtual DateTime? UpdatedAt { get; set; }
    public virtual bool IsDeleted { get; set; }
}