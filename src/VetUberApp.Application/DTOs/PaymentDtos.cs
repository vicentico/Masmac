using System.ComponentModel.DataAnnotations;
using VetUberApp.Domain.Enums;

namespace VetUberApp.Application.DTOs;

/// <summary>
/// DTO para crear un nuevo pago
/// </summary>
public class CreatePaymentDto
{
    /// <summary>
    /// ID de la cita asociada al pago
    /// </summary>
    [Required]
    public string AppointmentId { get; set; } = default!;

    /// <summary>
    /// Monto del pago
    /// </summary>
    [Required]
    [Range(0.01, double.MaxValue, ErrorMessage = "El monto debe ser mayor a 0")]
    public decimal Amount { get; set; }

    /// <summary>
    /// Método de pago seleccionado
    /// </summary>
    [Required]
    public PaymentMethod Method { get; set; }

    /// <summary>
    /// ID de transacción externo (opcional)
    /// </summary>
    public string? TransactionId { get; set; }
}

/// <summary>
/// DTO para actualizar un pago existente
/// </summary>
public class UpdatePaymentDto
{
    /// <summary>
    /// Nuevo estado del pago
    /// </summary>
    public PaymentStatus? Status { get; set; }

    /// <summary>
    /// ID de transacción actualizado
    /// </summary>
    public string? TransactionId { get; set; }

    /// <summary>
    /// Razón del fallo en caso de error
    /// </summary>
    public string? FailureReason { get; set; }

    /// <summary>
    /// Razón del reembolso en caso de devolución
    /// </summary>
    public string? RefundReason { get; set; }
}

/// <summary>
/// DTO completo para un pago
/// </summary>
public class PaymentDto
{
    /// <summary>
    /// ID único del pago
    /// </summary>
    public string Id { get; set; } = default!;

    /// <summary>
    /// Información básica de la cita asociada
    /// </summary>
    public AppointmentBasicDto Appointment { get; set; } = default!;

    /// <summary>
    /// Monto del pago
    /// </summary>
    public decimal Amount { get; set; }

    /// <summary>
    /// Estado actual del pago
    /// </summary>
    public PaymentStatus Status { get; set; }

    /// <summary>
    /// Método de pago utilizado
    /// </summary>
    public PaymentMethod Method { get; set; }

    /// <summary>
    /// ID de transacción externo
    /// </summary>
    public string? TransactionId { get; set; }

    /// <summary>
    /// Fecha y hora en que se completó el pago
    /// </summary>
    public DateTime? PaidAt { get; set; }

    /// <summary>
    /// Razón del fallo si el pago falló
    /// </summary>
    public string? FailureReason { get; set; }

    /// <summary>
    /// Razón del reembolso si el pago fue reembolsado
    /// </summary>
    public string? RefundReason { get; set; }

    /// <summary>
    /// Fecha de creación del registro de pago
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Última fecha de actualización del pago
    /// </summary>
    public DateTime? UpdatedAt { get; set; }
}