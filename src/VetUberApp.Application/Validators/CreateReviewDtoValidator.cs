using FluentValidation;
using VetUberApp.Application.DTOs;

namespace VetUberApp.Application.Validators;

/// <summary>
/// Validador para el DTO de creación de reseñas
/// </summary>
public class CreateReviewDtoValidator : AbstractValidator<CreateReviewDto>
{
    public CreateReviewDtoValidator()
    {
        RuleFor(x => x.AppointmentId)
            .NotEmpty().WithMessage("El ID de la cita es requerido")
            .Length(24).WithMessage("El ID debe tener 24 caracteres (formato MongoDB)");

        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("El ID del usuario es requerido")
            .Length(24).WithMessage("El ID debe tener 24 caracteres (formato MongoDB)");

        RuleFor(x => x.VeterinarianId)
            .NotEmpty().WithMessage("El ID del veterinario es requerido")
            .Length(24).WithMessage("El ID debe tener 24 caracteres (formato MongoDB)");

        RuleFor(x => x.Rating)
            .InclusiveBetween(1.00m, 5.00m).WithMessage("La calificación debe estar entre 1.00 y 5.00")
            .Must(rating => decimal.Round(rating, 2) == rating)
                .WithMessage("La calificación debe tener máximo 2 decimales");

        RuleFor(x => x.Comment)
            .NotEmpty().WithMessage("El comentario es requerido")
            .MinimumLength(10).WithMessage("El comentario debe tener al menos 10 caracteres")
            .MaximumLength(1000).WithMessage("El comentario no puede exceder los 1000 caracteres");

        RuleFor(x => x.Type)
            .IsInEnum().WithMessage("El tipo de reseña no es válido");
    }
}