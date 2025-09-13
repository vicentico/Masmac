using VetUberApp.Domain.Enums;

namespace VetUberApp.Application.DTOs;

public class AppointmentBasicDto
{
    public string Id { get; set; } = default!;
    public string VeterinarianName { get; set; } = default!;
    public string PetName { get; set; } = default!;
    public DateTime ScheduledDateTime { get; set; }
    public AppointmentStatus Status { get; set; }
}