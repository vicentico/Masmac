using VetUberApp.Application.DTOs;
using VetUberApp.Application.Extensions;
using VetUberApp.Application.Interfaces;
using VetUberApp.Domain.Entities;
using VetUberApp.Domain.Enums;
using VetUberApp.Domain.Interfaces;
using VetUberApp.Domain.ValueObjects;

namespace VetUberApp.Application.Services;

public class AppointmentService : IAppointmentService
{
    private readonly IAppointmentRepository _appointmentRepository;
    private readonly IVeterinarianRepository _veterinarianRepository;
    private readonly IUserRepository _userRepository;
    private readonly IPetRepository _petRepository;

    public AppointmentService(
        IAppointmentRepository appointmentRepository,
        IVeterinarianRepository veterinarianRepository,
        IUserRepository userRepository,
        IPetRepository petRepository)
    {
        _appointmentRepository = appointmentRepository;
        _veterinarianRepository = veterinarianRepository;
        _userRepository = userRepository;
        _petRepository = petRepository;
    }

    public async Task<AppointmentDto> CreateAsync(CreateAppointmentDto dto)
    {
        // Verificar que el veterinario existe y está disponible
        var veterinarian = await _veterinarianRepository.GetByIdAsync(dto.VeterinarianId);
        if (veterinarian == null)
            throw new InvalidOperationException("El veterinario especificado no existe.");

        var isAvailable = await _appointmentRepository.IsVeterinarianAvailableAsync(
            dto.VeterinarianId, 
            dto.ScheduledDateTime);

        if (!isAvailable)
            throw new InvalidOperationException("El veterinario no está disponible en la fecha y hora especificada.");

        // Verificar que el dueño existe
        var owner = await _userRepository.GetByIdAsync(dto.OwnerId);
        if (owner == null)
            throw new InvalidOperationException("El dueño especificado no existe.");

        // Verificar que la mascota existe y pertenece al dueño
        var pet = await _petRepository.GetByIdAsync(dto.PetId);
        if (pet == null)
            throw new InvalidOperationException("La mascota especificada no existe.");

        if (pet.OwnerId != dto.OwnerId)
            throw new InvalidOperationException("La mascota no pertenece al dueño especificado.");

        // Crear la cita
        var appointment = new Appointment
        {
            VeterinarianId = dto.VeterinarianId,
            OwnerId = dto.OwnerId,
            PetId = dto.PetId,
            ScheduledDateTime = dto.ScheduledDateTime,
            Reason = dto.Reason,
            Type = dto.Type,
            Status = AppointmentStatus.Pending,
            Location = dto.Location?.ToEntity()
        };

        await _appointmentRepository.CreateAsync(appointment);

        // Retornar el DTO con toda la información
        return await GetAppointmentDtoAsync(appointment);
    }

    public async Task<AppointmentDto?> GetByIdAsync(string id)
    {
        var appointment = await _appointmentRepository.GetByIdAsync(id);
        if (appointment == null)
            return null;

        return await GetAppointmentDtoAsync(appointment);
    }

    public async Task<IEnumerable<AppointmentDto>> GetAllAsync()
    {
        var appointments = await _appointmentRepository.GetAllAsync();
        var dtos = new List<AppointmentDto>();

        foreach (var appointment in appointments)
        {
            dtos.Add(await GetAppointmentDtoAsync(appointment));
        }

        return dtos;
    }

    public async Task<IEnumerable<AppointmentDto>> GetByVeterinarianIdAsync(string veterinarianId)
    {
        var appointments = await _appointmentRepository.GetByVeterinarianIdAsync(veterinarianId);
        var dtos = new List<AppointmentDto>();

        foreach (var appointment in appointments)
        {
            dtos.Add(await GetAppointmentDtoAsync(appointment));
        }

        return dtos;
    }

    public async Task<IEnumerable<AppointmentDto>> GetByUserIdAsync(string userId)
    {
        var appointments = await _appointmentRepository.GetByOwnerIdAsync(userId);
        var dtos = new List<AppointmentDto>();

        foreach (var appointment in appointments)
        {
            dtos.Add(await GetAppointmentDtoAsync(appointment));
        }

        return dtos;
    }

    public async Task<IEnumerable<AppointmentDto>> GetByPetIdAsync(string petId)
    {
        var appointments = await _appointmentRepository.GetByPetIdAsync(petId);
        var dtos = new List<AppointmentDto>();

        foreach (var appointment in appointments)
        {
            dtos.Add(await GetAppointmentDtoAsync(appointment));
        }

        return dtos;
    }

    public async Task<AppointmentDto> UpdateAsync(string id, UpdateAppointmentDto dto)
    {
        var appointment = await _appointmentRepository.GetByIdAsync(id);
        if (appointment == null)
            throw new InvalidOperationException("La cita especificada no existe.");

        // Si se está actualizando la fecha/hora, verificar disponibilidad del veterinario
        if (dto.ScheduledDateTime.HasValue)
        {
            var newScheduledDateTime = dto.ScheduledDateTime.Value;
            var isAvailable = await _appointmentRepository.IsVeterinarianAvailableAsync(
                appointment.VeterinarianId,
                newScheduledDateTime);

            if (!isAvailable)
                throw new InvalidOperationException("El veterinario no está disponible en la fecha y hora especificada.");

            appointment.ScheduledDateTime = newScheduledDateTime;
        }

        // Actualizar los demás campos si se proporcionaron
        if (dto.Reason != null)
            appointment.Reason = dto.Reason;

        if (dto.Type.HasValue)
            appointment.Type = dto.Type.Value;

        if (dto.Status.HasValue)
            appointment.Status = dto.Status.Value;

        if (dto.Location != null)
            appointment.Location = dto.Location.ToEntity();

        appointment.UpdatedAt = DateTime.UtcNow;
        await _appointmentRepository.UpdateAsync(appointment);

        return await GetAppointmentDtoAsync(appointment);
    }

    public async Task DeleteAsync(string id)
    {
        await _appointmentRepository.DeleteAsync(id);
    }

    private async Task<AppointmentDto> GetAppointmentDtoAsync(Appointment appointment)
    {
        var veterinarian = await _veterinarianRepository.GetByIdAsync(appointment.VeterinarianId);
        var owner = await _userRepository.GetByIdAsync(appointment.OwnerId);
        var pet = await _petRepository.GetByIdAsync(appointment.PetId);

        if (veterinarian == null || owner == null || pet == null)
            throw new InvalidOperationException("No se encontraron todas las entidades relacionadas con la cita.");

        return new AppointmentDto
        {
            Id = appointment.Id,
            Veterinarian = veterinarian.ToBasicDto(),
            Owner = owner.ToBasicDto(),
            Pet = pet.ToBasicDto(),
            ScheduledDateTime = appointment.ScheduledDateTime,
            Reason = appointment.Reason,
            Type = appointment.Type,
            Status = appointment.Status,
            Location = appointment.Location?.ToDto(),
            CreatedAt = appointment.CreatedAt,
            UpdatedAt = appointment.UpdatedAt
        };
    }
}