using FluentValidation;
using VetUberApp.Application.DTOs;
using VetUberApp.Domain.Enums;

namespace VetUberApp.Application.Validators;

public class UpdatePaymentDtoValidator : AbstractValidator<UpdatePaymentDto>
{
    public UpdatePaymentDtoValidator()
    {
        RuleFor(x => x.Status)
            .IsInEnum().WithMessage("El estado de pago especificado no es válido")
            .When(x => x.Status.HasValue);

        When(x => x.Status == PaymentStatus.Failed, () =>
        {
            RuleFor(x => x.FailureReason)
                .NotEmpty().WithMessage("Se debe especificar el motivo del fallo del pago");
        });

        When(x => x.Status == PaymentStatus.Refunded, () =>
        {
            RuleFor(x => x.RefundReason)
                .NotEmpty().WithMessage("Se debe especificar el motivo del reembolso");
        });

        When(x => x.TransactionId != null, () =>
        {
            RuleFor(x => x.TransactionId)
                .MinimumLength(5).WithMessage("El ID de transacción debe tener al menos 5 caracteres");
        });
    }
}