using VetUberApp.Domain.Common;
using VetUberApp.Domain.Enums;
using VetUberApp.Domain.ValueObjects;

namespace VetUberApp.Domain.Entities;

public class User : BaseEntity
{
    public string Email { get; set; } = default!;
    public string PasswordHash { get; set; } = default!;
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string PhoneNumber { get; set; } = default!;
    public string FullName => $"{FirstName} {LastName}";
    public string? ProfilePictureUrl { get; set; }
    public UserRole Role { get; set; }
    public bool IsVerified { get; set; }
    public Address Address { get; set; } = default!;
    public List<Pet> Pets { get; set; } = new();
    public List<Review> ReviewsGiven { get; set; } = new();
    public List<Appointment> Appointments { get; set; } = new();
}