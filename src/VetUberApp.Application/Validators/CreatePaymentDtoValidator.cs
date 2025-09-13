using FluentValidation;
using VetUberApp.Application.DTOs;
using VetUberApp.Domain.Interfaces;

namespace VetUberApp.Application.Validators;

public class CreatePaymentDtoValidator : AbstractValidator<CreatePaymentDto>
{
    private readonly IAppointmentRepository _appointmentRepository;

    public CreatePaymentDtoValidator(IAppointmentRepository appointmentRepository)
    {
        _appointmentRepository = appointmentRepository;

        RuleFor(x => x.AppointmentId)
            .NotEmpty().WithMessage("El ID de la cita es requerido")
            .MustAsync(async (appointmentId, cancellation) =>
            {
                var appointment = await _appointmentRepository.GetByIdAsync(appointmentId);
                return appointment != null;
            }).WithMessage("La cita especificada no existe");

        RuleFor(x => x.Amount)
            .NotEmpty().WithMessage("El monto es requerido")
            .GreaterThan(0).WithMessage("El monto debe ser mayor a 0")
            .PrecisionScale(10, 2, false).WithMessage("El monto debe tener máximo 2 decimales");

        RuleFor(x => x.Method)
            .IsInEnum().WithMessage("El método de pago especificado no es válido");

        When(x => x.Method == Domain.Enums.PaymentMethod.CreditCard, () =>
        {
            RuleFor(x => x.TransactionId)
                .NotEmpty().WithMessage("El ID de transacción es requerido para pagos con tarjeta de crédito");
        });
    }
}