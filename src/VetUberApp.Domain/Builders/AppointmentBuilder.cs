using VetUberApp.Domain.Entities;
using VetUberApp.Domain.Enums;
using VetUberApp.Domain.ValueObjects;

namespace VetUberApp.Domain.Builders;

/// <summary>
/// Builder para crear instancias de Appointment de manera fluida y consistente
/// </summary>
public class AppointmentBuilder
{
    private readonly Appointment _appointment;

    public AppointmentBuilder()
    {
        _appointment = new Appointment();
    }

    /// <summary>
    /// Establece el dueño de la mascota
    /// </summary>
    /// <param name="ownerId">ID del dueño</param>
    /// <param name="owner">Entidad del dueño (opcional)</param>
    /// <returns>Builder para encadenamiento</returns>
    public AppointmentBuilder WithOwner(string ownerId, User? owner = null)
    {
        if (string.IsNullOrWhiteSpace(ownerId))
            throw new ArgumentException("El ID del dueño no puede ser null o vacío", nameof(ownerId));

        _appointment.OwnerId = ownerId;
        if (owner != null)
            _appointment.Owner = owner;

        return this;
    }

    /// <summary>
    /// Establece el veterinario
    /// </summary>
    /// <param name="veterinarianId">ID del veterinario</param>
    /// <param name="veterinarian">Entidad del veterinario (opcional)</param>
    /// <returns>Builder para encadenamiento</returns>
    public AppointmentBuilder WithVeterinarian(string veterinarianId, Veterinarian? veterinarian = null)
    {
        if (string.IsNullOrWhiteSpace(veterinarianId))
            throw new ArgumentException("El ID del veterinario no puede ser null o vacío", nameof(veterinarianId));

        _appointment.VeterinarianId = veterinarianId;
        if (veterinarian != null)
            _appointment.Veterinarian = veterinarian;

        return this;
    }

    /// <summary>
    /// Establece la mascota
    /// </summary>
    /// <param name="petId">ID de la mascota</param>
    /// <param name="pet">Entidad de la mascota (opcional)</param>
    /// <returns>Builder para encadenamiento</returns>
    public AppointmentBuilder WithPet(string petId, Pet? pet = null)
    {
        if (string.IsNullOrWhiteSpace(petId))
            throw new ArgumentException("El ID de la mascota no puede ser null o vacío", nameof(petId));

        _appointment.PetId = petId;
        if (pet != null)
            _appointment.Pet = pet;

        return this;
    }

    /// <summary>
    /// Establece la fecha y hora programada
    /// </summary>
    /// <param name="scheduledDateTime">Fecha y hora de la cita</param>
    /// <returns>Builder para encadenamiento</returns>
    public AppointmentBuilder ScheduledFor(DateTime scheduledDateTime)
    {
        if (scheduledDateTime <= DateTime.Now)
            throw new ArgumentException("La fecha de la cita debe ser futura", nameof(scheduledDateTime));

        _appointment.ScheduledDateTime = scheduledDateTime;
        return this;
    }

    /// <summary>
    /// Establece el motivo de la consulta
    /// </summary>
    /// <param name="reason">Motivo de la consulta</param>
    /// <returns>Builder para encadenamiento</returns>
    public AppointmentBuilder WithReason(string reason)
    {
        if (string.IsNullOrWhiteSpace(reason))
            throw new ArgumentException("El motivo no puede ser null o vacío", nameof(reason));

        _appointment.Reason = reason;
        return this;
    }

    /// <summary>
    /// Establece el tipo de cita
    /// </summary>
    /// <param name="type">Tipo de cita</param>
    /// <returns>Builder para encadenamiento</returns>
    public AppointmentBuilder OfType(AppointmentType type)
    {
        _appointment.Type = type;
        return this;
    }

    /// <summary>
    /// Establece el estado de la cita
    /// </summary>
    /// <param name="status">Estado de la cita</param>
    /// <returns>Builder para encadenamiento</returns>
    public AppointmentBuilder WithStatus(AppointmentStatus status)
    {
        _appointment.Status = status;
        return this;
    }

    /// <summary>
    /// Establece la ubicación de la cita
    /// </summary>
    /// <param name="location">Dirección donde se realizará la cita</param>
    /// <returns>Builder para encadenamiento</returns>
    public AppointmentBuilder AtLocation(Address location)
    {
        _appointment.Location = location;
        return this;
    }

    /// <summary>
    /// Establece el diagnóstico
    /// </summary>
    /// <param name="diagnosis">Diagnóstico de la consulta</param>
    /// <returns>Builder para encadenamiento</returns>
    public AppointmentBuilder WithDiagnosis(string diagnosis)
    {
        if (string.IsNullOrWhiteSpace(diagnosis))
            throw new ArgumentException("El diagnóstico no puede ser null o vacío", nameof(diagnosis));

        _appointment.Diagnosis = diagnosis;
        return this;
    }

    /// <summary>
    /// Establece la prescripción
    /// </summary>
    /// <param name="prescription">Prescripción médica</param>
    /// <returns>Builder para encadenamiento</returns>
    public AppointmentBuilder WithPrescription(string prescription)
    {
        if (string.IsNullOrWhiteSpace(prescription))
            throw new ArgumentException("La prescripción no puede ser null o vacía", nameof(prescription));

        _appointment.Prescription = prescription;
        return this;
    }

    /// <summary>
    /// Agrega fotos a la cita
    /// </summary>
    /// <param name="photos">URLs de las fotos</param>
    /// <returns>Builder para encadenamiento</returns>
    public AppointmentBuilder WithPhotos(params string[] photos)
    {
        if (photos?.Any() == true)
        {
            _appointment.Photos = photos.Where(p => !string.IsNullOrWhiteSpace(p)).ToList();
        }
        return this;
    }

    /// <summary>
    /// Construye la instancia de Appointment
    /// </summary>
    /// <returns>Instancia de Appointment configurada</returns>
    /// <exception cref="InvalidOperationException">Si faltan propiedades requeridas</exception>
    public Appointment Build()
    {
        ValidateRequiredFields();
        return _appointment;
    }

    private void ValidateRequiredFields()
    {
        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(_appointment.OwnerId))
            errors.Add("El ID del dueño es requerido");

        if (string.IsNullOrWhiteSpace(_appointment.VeterinarianId))
            errors.Add("El ID del veterinario es requerido");

        if (string.IsNullOrWhiteSpace(_appointment.PetId))
            errors.Add("El ID de la mascota es requerido");

        if (_appointment.ScheduledDateTime == default)
            errors.Add("La fecha programada es requerida");

        if (string.IsNullOrWhiteSpace(_appointment.Reason))
            errors.Add("El motivo es requerido");

        if (errors.Any())
            throw new InvalidOperationException($"Faltan campos requeridos: {string.Join(", ", errors)}");
    }
}