namespace VetUberApp.Domain.ValueObjects;

/// <summary>
/// Representa una dirección física completa con coordenadas geográficas
/// </summary>
public class Address
{
    /// <summary>Nombre de la calle y número</summary>
    public string Street { get; set; } = default!;
    
    /// <summary>Ciudad</summary>
    public string City { get; set; } = default!;
    
    /// <summary>Estado o provincia</summary>
    public string State { get; set; } = default!;
    
    /// <summary>País</summary>
    public string Country { get; set; } = default!;
    
    /// <summary>Código postal</summary>
    public string ZipCode { get; set; } = default!;
    
    /// <summary>Latitud de la ubicación</summary>
    public double Latitude { get; set; }
    
    /// <summary>Longitud de la ubicación</summary>
    public double Longitude { get; set; }

    /// <summary>Información adicional para encontrar la dirección</summary>
    public string? AdditionalInfo { get; set; }
}