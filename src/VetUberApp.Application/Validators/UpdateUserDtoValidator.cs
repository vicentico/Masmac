using FluentValidation;
using VetUberApp.Application.DTOs;

namespace VetUberApp.Application.Validators;

/// <summary>
/// Validador para UpdateUserDto que define las reglas de negocio
/// para la actualización de usuarios
/// </summary>
public class UpdateUserDtoValidator : AbstractValidator<UpdateUserDto>
{
    public UpdateUserDtoValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("El nombre es requerido.")
            .MaximumLength(50).WithMessage("El nombre no puede exceder los 50 caracteres.");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("El apellido es requerido.")
            .MaximumLength(50).WithMessage("El apellido no puede exceder los 50 caracteres.");

        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("El número de teléfono es requerido.")
            .Matches(@"^\+?[1-9]\d{1,14}$").WithMessage("El número de teléfono debe tener un formato válido.");

        RuleFor(x => x.ProfilePictureUrl)
            .Must(BeAValidUrl).WithMessage("La URL de la foto de perfil debe ser válida.")
            .When(x => !string.IsNullOrEmpty(x.ProfilePictureUrl));

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

    private static bool BeAValidUrl(string? url)
    {
        if (string.IsNullOrEmpty(url))
            return true;

        return Uri.TryCreate(url, UriKind.Absolute, out var result) &&
               (result.Scheme == Uri.UriSchemeHttp || result.Scheme == Uri.UriSchemeHttps);
    }
}