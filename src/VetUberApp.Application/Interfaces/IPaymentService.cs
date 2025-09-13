using VetUberApp.Application.DTOs;

namespace VetUberApp.Application.Interfaces;

public interface IPaymentService
{
    Task<PaymentDto> CreateAsync(CreatePaymentDto dto);
    Task<PaymentDto?> GetByIdAsync(string id);
    Task<PaymentDto?> GetByAppointmentIdAsync(string appointmentId);
    Task<IEnumerable<PaymentDto>> GetAllAsync();
    Task<PaymentDto> UpdateAsync(string id, UpdatePaymentDto dto);
    Task<PaymentDto> ProcessPaymentAsync(string id);
    Task<PaymentDto> RefundPaymentAsync(string id, string reason);
}