using FluentValidation;
using VetUberApp.Application.DTOs;
using VetUberApp.Domain.Constants;

namespace VetUberApp.Application.Validators;

/// <summary>
/// Validador para el DTO de creación de reseñas
/// </summary>
public class CreateReviewDtoValidator : AbstractValidator<CreateReviewDto>
{
    public CreateReviewDtoValidator()
    {
        RuleFor(x => x.AppointmentId)
            .NotEmpty().WithMessage(ErrorMessages.RequiredField)
            .Length(ValidationConstants.MongoObjectIdLength).WithMessage(ErrorMessages.InvalidObjectId);

        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage(ErrorMessages.RequiredField)
            .Length(ValidationConstants.MongoObjectIdLength).WithMessage(ErrorMessages.InvalidObjectId);

        RuleFor(x => x.VeterinarianId)
            .NotEmpty().WithMessage(ErrorMessages.RequiredField)
            .Length(ValidationConstants.MongoObjectIdLength).WithMessage(ErrorMessages.InvalidObjectId);

        RuleFor(x => x.Rating)
            .InclusiveBetween(ValidationConstants.MinRating, ValidationConstants.MaxRating)
                .WithMessage(ErrorMessages.InvalidRatingRange)
            .Must(rating => decimal.Round(rating, ValidationConstants.RatingDecimalPlaces) == rating)
                .WithMessage(ErrorMessages.InvalidRatingDecimals);

        RuleFor(x => x.Comment)
            .NotEmpty().WithMessage(ErrorMessages.RequiredField)
            .Length(ValidationConstants.MinCommentLength, ValidationConstants.MaxCommentLength)
                .WithMessage(ErrorMessages.InvalidCommentLength);

        RuleFor(x => x.Type)
            .IsInEnum().WithMessage("El tipo de reseña no es válido");
    }
}