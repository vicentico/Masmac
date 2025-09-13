using VetUberApp.Domain.Common;
using VetUberApp.Domain.Enums;

namespace VetUberApp.Domain.Entities;

public class Review : BaseEntity
{
    public string AppointmentId { get; set; } = null!;
    public string UserId { get; set; } = null!;
    public string VeterinarianId { get; set; } = null!;
    public decimal Rating { get; set; }
    public string Comment { get; set; } = null!;
    public ReviewType Type { get; set; }
}