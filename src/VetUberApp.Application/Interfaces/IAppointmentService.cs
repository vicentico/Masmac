using VetUberApp.Application.DTOs;

namespace VetUberApp.Application.Interfaces;

public interface IAppointmentService
{
    Task<AppointmentDto> CreateAsync(CreateAppointmentDto dto);
    Task<AppointmentDto?> GetByIdAsync(string id);
    Task<IEnumerable<AppointmentDto>> GetAllAsync();
    Task<IEnumerable<AppointmentDto>> GetByUserIdAsync(string userId);
    Task<IEnumerable<AppointmentDto>> GetByVeterinarianIdAsync(string veterinarianId);
    Task<AppointmentDto> UpdateAsync(string id, UpdateAppointmentDto dto);
    Task DeleteAsync(string id);
}