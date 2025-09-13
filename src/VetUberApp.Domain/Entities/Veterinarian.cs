using VetUberApp.Domain.Common;
using VetUberApp.Domain.ValueObjects;

namespace VetUberApp.Domain.Entities;

public class Veterinarian : BaseEntity
{
    public string Email { get; set; } = default!;
    public string PasswordHash { get; set; } = default!;
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string PhoneNumber { get; set; } = default!;
    public string FullName => $"{FirstName} {LastName}";
    public string Specialization => string.Join(", ", Specialties);
    public string? ProfilePictureUrl { get; set; }
    public string LicenseNumber { get; set; } = default!;
    public List<string> Specialties { get; set; } = new();
    public string? Biography { get; set; }
    public bool IsVerified { get; set; }
    public bool IsAvailable { get; set; }
    public Address CurrentLocation { get; set; } = default!;
    public decimal Rating { get; set; }
    public List<Review> Reviews { get; set; } = new();
    public List<Appointment> Appointments { get; set; } = new();
    public decimal ConsultationFee { get; set; }
}