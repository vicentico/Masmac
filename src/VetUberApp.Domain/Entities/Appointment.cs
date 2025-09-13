using VetUberApp.Domain.Common;
using VetUberApp.Domain.Enums;
using VetUberApp.Domain.ValueObjects;

namespace VetUberApp.Domain.Entities;

public class Appointment : BaseEntity
{
    public string OwnerId { get; set; } = default!;
    public User Owner { get; set; } = default!;
    public string VeterinarianId { get; set; } = default!;
    public Veterinarian Veterinarian { get; set; } = default!;
    public string PetId { get; set; } = default!;
    public Pet Pet { get; set; } = default!;
    public DateTime ScheduledDateTime { get; set; }
    public DateTime? StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public string Reason { get; set; } = default!;
    public AppointmentType Type { get; set; }
    public AppointmentStatus Status { get; set; }
    public Address? Location { get; set; }
    public string? Diagnosis { get; set; }
    public string? Prescription { get; set; }
    public List<string>? Photos { get; set; }
    public Payment? Payment { get; set; }
    public Review? Review { get; set; }
}