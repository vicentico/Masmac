using VetUberApp.Application.DTOs;
using VetUberApp.Application.Extensions;
using VetUberApp.Application.Interfaces;
using VetUberApp.Domain.Entities;
using VetUberApp.Domain.Enums;
using VetUberApp.Domain.Interfaces;

namespace VetUberApp.Application.Services;

public class PaymentService : IPaymentService
{
    private readonly IPaymentRepository _paymentRepository;
    private readonly IAppointmentRepository _appointmentRepository;

    public PaymentService(
        IPaymentRepository paymentRepository,
        IAppointmentRepository appointmentRepository)
    {
        _paymentRepository = paymentRepository;
        _appointmentRepository = appointmentRepository;
    }

    public async Task<PaymentDto> CreateAsync(CreatePaymentDto dto)
    {
        var appointment = await _appointmentRepository.GetByIdAsync(dto.AppointmentId)
            ?? throw new InvalidOperationException("La cita especificada no existe.");

        var payment = new Payment
        {
            AppointmentId = dto.AppointmentId,
            Appointment = appointment,
            Amount = dto.Amount,
            Method = dto.Method,
            Status = PaymentStatus.Pending,
            TransactionId = dto.TransactionId
        };

        await _paymentRepository.CreateAsync(payment);

        return await GetPaymentDtoAsync(payment);
    }

    public async Task<PaymentDto?> GetByIdAsync(string id)
    {
        var payment = await _paymentRepository.GetByIdAsync(id);
        if (payment == null)
            return null;

        return await GetPaymentDtoAsync(payment);
    }

    public async Task<PaymentDto?> GetByAppointmentIdAsync(string appointmentId)
    {
        var payment = await _paymentRepository.GetByAppointmentIdAsync(appointmentId);
        if (payment == null)
            return null;

        return await GetPaymentDtoAsync(payment);
    }

    public async Task<IEnumerable<PaymentDto>> GetAllAsync()
    {
        var payments = await _paymentRepository.GetAllAsync();
        var dtos = new List<PaymentDto>();

        foreach (var payment in payments)
        {
            dtos.Add(await GetPaymentDtoAsync(payment));
        }

        return dtos;
    }

    public async Task<PaymentDto> UpdateAsync(string id, UpdatePaymentDto dto)
    {
        var payment = await _paymentRepository.GetByIdAsync(id)
            ?? throw new InvalidOperationException("El pago especificado no existe.");

        if (dto.Status.HasValue)
            payment.Status = dto.Status.Value;

        if (dto.TransactionId != null)
            payment.TransactionId = dto.TransactionId;

        if (dto.FailureReason != null)
            payment.FailureReason = dto.FailureReason;

        if (dto.RefundReason != null)
            payment.RefundReason = dto.RefundReason;

        await _paymentRepository.UpdateAsync(payment);

        return await GetPaymentDtoAsync(payment);
    }

    public async Task<PaymentDto> ProcessPaymentAsync(string id)
    {
        var payment = await _paymentRepository.GetByIdAsync(id)
            ?? throw new InvalidOperationException("El pago especificado no existe.");

        if (payment.Status != PaymentStatus.Pending)
            throw new InvalidOperationException("Solo se pueden procesar pagos pendientes.");

        // Aquí iría la lógica de integración con el proveedor de pagos
        // Por ahora solo simulamos el proceso

        payment.Status = PaymentStatus.Completed;
            payment.PaidAt = DateTime.UtcNow;
            payment.UpdatedAt = DateTime.UtcNow;        await _paymentRepository.UpdateAsync(payment);

        return await GetPaymentDtoAsync(payment);
    }

    public async Task<PaymentDto> RefundPaymentAsync(string id, string reason)
    {
        var payment = await _paymentRepository.GetByIdAsync(id)
            ?? throw new InvalidOperationException("El pago especificado no existe.");

        if (payment.Status != PaymentStatus.Completed)
            throw new InvalidOperationException("Solo se pueden reembolsar pagos completados.");

        // Aquí iría la lógica de integración con el proveedor de pagos
        // Por ahora solo simulamos el proceso

        payment.Status = PaymentStatus.Refunded;
        payment.RefundReason = reason;

        await _paymentRepository.UpdateAsync(payment);

        return await GetPaymentDtoAsync(payment);
    }

    private async Task<PaymentDto> GetPaymentDtoAsync(Payment payment)
    {
        var appointment = await _appointmentRepository.GetByIdAsync(payment.AppointmentId)
            ?? throw new InvalidOperationException("No se encontró la cita asociada al pago.");

        return new PaymentDto
        {
            Id = payment.Id,
            Appointment = new AppointmentBasicDto
            {
                Id = appointment.Id,
                VeterinarianName = appointment.Veterinarian?.FullName ?? "Desconocido",
                PetName = appointment.Pet?.Name ?? "Desconocido",
                ScheduledDateTime = appointment.ScheduledDateTime,
                Status = appointment.Status
            },
            Amount = payment.Amount,
            Status = payment.Status,
            Method = payment.Method,
            TransactionId = payment.TransactionId,
            PaidAt = payment.PaidAt,
            FailureReason = payment.FailureReason,
            RefundReason = payment.RefundReason,
            CreatedAt = payment.CreatedAt,
            UpdatedAt = payment.UpdatedAt
        };
    }
}