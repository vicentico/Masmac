using FluentValidation;
using VetUberApp.Application.DTOs;
using VetUberApp.Domain.Constants;

namespace VetUberApp.Application.Validators;

/// <summary>
/// Validador para el DTO de actualización de reseñas
/// </summary>
public class UpdateReviewDtoValidator : AbstractValidator<UpdateReviewDto>
{
    public UpdateReviewDtoValidator()
    {
        RuleFor(x => x.Rating)
            .InclusiveBetween(ValidationConstants.MinRating, ValidationConstants.MaxRating)
            .When(x => x.Rating.HasValue)
            .WithMessage(ErrorMessages.InvalidRatingRange)
            .Must(rating => rating == null || decimal.Round(rating.Value, ValidationConstants.RatingDecimalPlaces) == rating.Value)
            .WithMessage(ErrorMessages.InvalidRatingDecimals);

        RuleFor(x => x.Comment)
            .Length(ValidationConstants.MinCommentLength, ValidationConstants.MaxCommentLength)
            .When(x => !string.IsNullOrEmpty(x.Comment))
            .WithMessage(ErrorMessages.InvalidCommentLength);

        RuleFor(x => x.Type)
            .IsInEnum()
            .When(x => x.Type.HasValue)
            .WithMessage("El tipo de reseña no es válido");
    }
}