using VetUberApp.Domain.Common;
using VetUberApp.Domain.Enums;

namespace VetUberApp.Domain.Entities;

public class Pet : BaseEntity
{
    public string Name { get; set; } = default!;
    public PetType Type { get; set; }
    public string Breed { get; set; } = default!;
    public DateTime DateOfBirth { get; set; }
    public string? PhotoUrl { get; set; }
    public double Weight { get; set; }
    public string? SpecialNotes { get; set; }
    public string OwnerId { get; set; } = default!;
    public User Owner { get; set; } = default!;
    public List<Appointment> MedicalHistory { get; set; } = new();
}