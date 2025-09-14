using FluentValidation;
using VetUberApp.Application.DTOs;

namespace VetUberApp.Application.Validators;

/// <summary>
/// Validador para CreateUserDto que define las reglas de negocio
/// para la creación de usuarios
/// </summary>
public class CreateUserDtoValidator : AbstractValidator<CreateUserDto>
{
    public CreateUserDtoValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("El email es requerido.")
            .EmailAddress().WithMessage("El email debe tener un formato válido.")
            .MaximumLength(100).WithMessage("El email no puede exceder los 100 caracteres.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("La contraseña es requerida.")
            .MinimumLength(6).WithMessage("La contraseña debe tener al menos 6 caracteres.")
            .MaximumLength(50).WithMessage("La contraseña no puede exceder los 50 caracteres.");

        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("El nombre es requerido.")
            .MaximumLength(50).WithMessage("El nombre no puede exceder los 50 caracteres.");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("El apellido es requerido.")
            .MaximumLength(50).WithMessage("El apellido no puede exceder los 50 caracteres.");

        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("El número de teléfono es requerido.")
            .Matches(@"^\+?[1-9]\d{1,14}$").WithMessage("El número de teléfono debe tener un formato válido.");

        RuleFor(x => x.Address)
            .NotNull().WithMessage("La dirección es requerida.");

        When(x => x.Address != null, () =>
        {
            RuleFor(x => x.Address.Street)
                .NotEmpty().WithMessage("La calle es requerida.")
                .MaximumLength(100).WithMessage("La calle no puede exceder los 100 caracteres.");

            RuleFor(x => x.Address.City)
                .NotEmpty().WithMessage("La ciudad es requerida.")
                .MaximumLength(50).WithMessage("La ciudad no puede exceder los 50 caracteres.");

            RuleFor(x => x.Address.ZipCode)
                .NotEmpty().WithMessage("El código postal es requerido.")
                .Matches(@"^\d{5}$").WithMessage("El código postal debe tener 5 dígitos.");
        });
    }
}