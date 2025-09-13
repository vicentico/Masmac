using VetUberApp.Domain.Common;
using VetUberApp.Domain.Enums;

namespace VetUberApp.Domain.Entities;

public class Payment : BaseEntity
{
    public string AppointmentId { get; set; } = default!;
    public Appointment Appointment { get; set; } = default!;
    public decimal Amount { get; set; }
    public PaymentStatus Status { get; set; }
    public PaymentMethod Method { get; set; }
    public string? TransactionId { get; set; }
    public DateTime? PaidAt { get; set; }
    public string? FailureReason { get; set; }
    public string? RefundReason { get; set; }
}