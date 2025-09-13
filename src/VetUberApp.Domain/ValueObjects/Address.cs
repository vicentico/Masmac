namespace VetUberApp.Domain.ValueObjects;

public class Address
{
    public string Street { get; set; } = default!;
    public string City { get; set; } = default!;
    public string State { get; set; } = default!;
    public string Country { get; set; } = default!;
    public string ZipCode { get; set; } = default!;
    public double Latitude { get; set; }
    public double Longitude { get; set; }
}