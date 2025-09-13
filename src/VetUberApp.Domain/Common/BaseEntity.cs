namespace VetUberApp.Domain.Common;

public abstract class BaseEntity
{
    public string Id { get; set; } = default!;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public bool IsDeleted { get; set; }
}