using VetUberApp.Application.DTOs;

namespace VetUberApp.Application.Interfaces;

public interface IPaymentService
{
    Task<PaymentDto> CreatePaymentAsync(CreatePaymentDto dto);
    Task<PaymentDto?> GetPaymentByIdAsync(string id);
    Task<PaymentDto?> GetPaymentByAppointmentIdAsync(string appointmentId);
    Task<IEnumerable<PaymentDto>> GetAllPaymentsAsync();
    Task<PaymentDto?> UpdatePaymentAsync(string id, UpdatePaymentDto dto);
    Task<PaymentDto?> ProcessPaymentAsync(string id);
    Task<PaymentDto?> RefundPaymentAsync(string id, string reason);
}