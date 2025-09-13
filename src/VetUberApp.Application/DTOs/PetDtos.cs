using VetUberApp.Domain.Enums;

namespace VetUberApp.Application.DTOs;

/// <summary>
/// DTO para crear una nueva mascota
/// </summary>
public record CreatePetDto(
    /// <summary>Nombre de la mascota</summary>
    string Name,
    /// <summary>Tipo de mascota (perro, gato, etc.)</summary>
    PetType Type,
    /// <summary>Raza de la mascota</summary>
    string Breed,
    /// <summary>Fecha de nacimiento</summary>
    DateTime DateOfBirth,
    /// <summary>URL de la foto (opcional)</summary>
    string? PhotoUrl,
    /// <summary>Peso en kilogramos</summary>
    double Weight,
    /// <summary>Notas especiales o condiciones médicas (opcional)</summary>
    string? SpecialNotes,
    /// <summary>ID del dueño de la mascota</summary>
    string OwnerId
);

/// <summary>
/// DTO para actualizar una mascota existente
/// </summary>
public record UpdatePetDto(
    /// <summary>Nombre de la mascota</summary>
    string Name,
    /// <summary>Raza de la mascota</summary>
    string Breed,
    /// <summary>URL de la foto (opcional)</summary>
    string? PhotoUrl,
    /// <summary>Peso en kilogramos</summary>
    double Weight,
    /// <summary>Notas especiales o condiciones médicas (opcional)</summary>
    string? SpecialNotes
);

/// <summary>
/// DTO con los datos completos de una mascota
/// </summary>
public record PetDto(
    /// <summary>Identificador único de la mascota</summary>
    string Id,
    /// <summary>Nombre de la mascota</summary>
    string Name,
    /// <summary>Tipo de mascota (perro, gato, etc.)</summary>
    PetType Type,
    /// <summary>Raza de la mascota</summary>
    string Breed,
    /// <summary>Fecha de nacimiento</summary>
    DateTime DateOfBirth,
    /// <summary>URL de la foto</summary>
    string? PhotoUrl,
    /// <summary>Peso en kilogramos</summary>
    double Weight,
    /// <summary>Notas especiales o condiciones médicas</summary>
    string? SpecialNotes,
    /// <summary>ID del dueño</summary>
    string OwnerId,
    /// <summary>Información básica del dueño</summary>
    UserBasicDto Owner
);