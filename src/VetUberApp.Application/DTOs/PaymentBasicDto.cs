using VetUberApp.Domain.Enums;

namespace VetUberApp.Application.DTOs;

public class PaymentBasicDto
{
    public string Id { get; set; } = default!;
    public decimal Amount { get; set; }
    public PaymentStatus Status { get; set; }
    public PaymentMethod Method { get; set; }
    public string? TransactionId { get; set; }
    public DateTime? PaidAt { get; set; }
}