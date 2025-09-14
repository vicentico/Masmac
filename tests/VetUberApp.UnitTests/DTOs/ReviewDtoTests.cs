using VetUberApp.Application.DTOs;
using VetUberApp.Application.Validators;
using VetUberApp.Domain.Enums;
using FluentAssertions;
using FluentValidation;

namespace VetUberApp.UnitTests.DTOs;

public class ReviewDtoTests
{
    private readonly CreateReviewDtoValidator _createValidator;
    private readonly UpdateReviewDtoValidator _updateValidator;

    public ReviewDtoTests()
    {
        _createValidator = new CreateReviewDtoValidator();
        _updateValidator = new UpdateReviewDtoValidator();
    }

    [Fact]
    public void CreateReviewDto_WithValidData_ShouldBeValid()
    {
        // Arrange
        var dto = new CreateReviewDto(
            AppointmentId: "123456789012345678901234", // 24 characters for MongoDB ObjectId
            UserId: "123456789012345678901234",
            VeterinarianId: "123456789012345678901234", 
            Rating: 4.5m,
            Comment: "Excellent service provided by the veterinarian",
            Type: ReviewType.Overall);

        // Act
        var result = _createValidator.Validate(dto);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("")]
    [InlineData("123")] // Too short
    public void CreateReviewDto_WithInvalidAppointmentId_ShouldBeInvalid(string appointmentId)
    {
        // Arrange
        var dto = new CreateReviewDto(
            AppointmentId: appointmentId,
            UserId: "123456789012345678901234",
            VeterinarianId: "123456789012345678901234",
            Rating: 5,
            Comment: "Excellent service provided",
            Type: ReviewType.Overall);

        // Act
        var result = _createValidator.Validate(dto);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "AppointmentId");
    }

    [Theory]
    [InlineData(0)]
    [InlineData(6)]
    public void CreateReviewDto_WithInvalidRating_ShouldBeInvalid(int rating)
    {
        // Arrange
        var dto = new CreateReviewDto(
            AppointmentId: "123456789012345678901234",
            UserId: "123456789012345678901234",
            VeterinarianId: "123456789012345678901234",
            Rating: rating,
            Comment: "Excellent service provided",
            Type: ReviewType.Overall);

        // Act
        var result = _createValidator.Validate(dto);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Rating");
    }

    [Fact]
    public void UpdateReviewDto_WithValidData_ShouldBeValid()
    {
        // Arrange
        var dto = new UpdateReviewDto
        {
            Rating = 4.0m,
            Comment = "Updated excellent service",
            Type = ReviewType.ProfessionalSkill
        };

        // Act
        var result = _updateValidator.Validate(dto);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(6)]
    public void UpdateReviewDto_WithInvalidRating_ShouldBeInvalid(int rating)
    {
        // Arrange
        var dto = new UpdateReviewDto
        {
            Rating = rating,
            Comment = "Updated service",
            Type = ReviewType.ProfessionalSkill
        };

        // Act
        var result = _updateValidator.Validate(dto);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Rating");
    }

    [Fact]
    public void UpdateReviewDto_WithNullValues_ShouldBeValid()
    {
        // Arrange
        var dto = new UpdateReviewDto();

        // Act
        var result = _updateValidator.Validate(dto);

        // Assert
        result.IsValid.Should().BeTrue();
    }
}