using VetUberApp.Domain.ValueObjects;

namespace VetUberApp.Application.DTOs;

public class AddressDto
{
    public string Street { get; set; } = null!;
    public string City { get; set; } = null!;
    public string State { get; set; } = null!;
    public string Country { get; set; } = null!;
    public string ZipCode { get; set; } = null!;
    public string? AdditionalInfo { get; set; }

    public static implicit operator Address(AddressDto dto)
    {
        return new Address
        {
            Street = dto.Street,
            City = dto.City,
            State = dto.State,
            Country = dto.Country,
            ZipCode = dto.ZipCode,
            AdditionalInfo = dto.AdditionalInfo
        };
    }

    public static implicit operator AddressDto(Address address)
    {
        return new AddressDto
        {
            Street = address.Street,
            City = address.City,
            State = address.State,
            Country = address.Country,
            ZipCode = address.ZipCode,
            AdditionalInfo = address.AdditionalInfo
        };
    }
}