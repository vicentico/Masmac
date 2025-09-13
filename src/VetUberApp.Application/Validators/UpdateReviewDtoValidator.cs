using FluentValidation;
using VetUberApp.Application.DTOs;

namespace VetUberApp.Application.Validators;

/// <summary>
/// Validador para el DTO de actualización de reseñas
/// </summary>
public class UpdateReviewDtoValidator : AbstractValidator<UpdateReviewDto>
{
    public UpdateReviewDtoValidator()
    {
        RuleFor(x => x.Rating)
            .InclusiveBetween(1.00m, 5.00m)
            .When(x => x.Rating.HasValue)
            .WithMessage("La calificación debe estar entre 1.00 y 5.00")
            .Must(rating => rating == null || decimal.Round(rating.Value, 2) == rating.Value)
            .WithMessage("La calificación debe tener máximo 2 decimales");

        RuleFor(x => x.Comment)
            .MinimumLength(10)
            .When(x => !string.IsNullOrEmpty(x.Comment))
            .WithMessage("El comentario debe tener al menos 10 caracteres")
            .MaximumLength(1000)
            .When(x => !string.IsNullOrEmpty(x.Comment))
            .WithMessage("El comentario no puede exceder los 1000 caracteres");

        RuleFor(x => x.Type)
            .IsInEnum()
            .When(x => x.Type.HasValue)
            .WithMessage("El tipo de reseña no es válido");
    }
}