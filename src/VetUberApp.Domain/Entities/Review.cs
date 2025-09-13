using VetUberApp.Domain.Common;

namespace VetUberApp.Domain.Entities;

public class Review : BaseEntity
{
    public int Rating { get; set; }
    public string Comment { get; set; } = default!;
    public string ReviewerId { get; set; } = default!;
    public User Reviewer { get; set; } = default!;
    public string VeterinarianId { get; set; } = default!;
    public Veterinarian Veterinarian { get; set; } = default!;
    public string AppointmentId { get; set; } = default!;
    public Appointment Appointment { get; set; } = default!;
}