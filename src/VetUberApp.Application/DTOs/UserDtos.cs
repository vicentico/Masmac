using VetUberApp.Domain.Entities;
using VetUberApp.Domain.Enums;
using VetUberApp.Domain.ValueObjects;

namespace VetUberApp.Application.DTOs;

public record CreateUserDto(
    string Email,
    string Password,
    string FirstName,
    string LastName,
    string PhoneNumber,
    Address Address
);

public record UpdateUserDto(
    string FirstName,
    string LastName,
    string PhoneNumber,
    string? ProfilePictureUrl,
    Address Address
);

public record UserDto(
    string Id,
    string Email,
    string FirstName,
    string LastName,
    string PhoneNumber,
    string? ProfilePictureUrl,
    UserRole Role,
    bool IsVerified,
    Address Address
);