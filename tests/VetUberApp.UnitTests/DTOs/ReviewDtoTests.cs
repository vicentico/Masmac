using VetUberApp.Application.DTOs;
using VetUberApp.Domain.Enums;
using FluentAssertions;
using System.ComponentModel.DataAnnotations;

namespace VetUberApp.UnitTests.DTOs;

public class ReviewDtoTests
{
    [Fact]
    public void CreateReviewDto_WithValidData_ShouldBeValid()
    {
        // Arrange
        var dto = new CreateReviewDto(
            AppointmentId: "appointment123",
            UserId: "user123",
            VeterinarianId: "vet123",
            Rating: 5,
            Comment: "Excellent service",
            Type: ReviewType.Overall);

        // Act
        var validationResults = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(dto, new ValidationContext(dto), validationResults, true);

        // Assert
        isValid.Should().BeTrue();
        validationResults.Should().BeEmpty();
    }

    [Theory]
    [InlineData("")]
    [InlineData(null!)]
    public void CreateReviewDto_WithInvalidAppointmentId_ShouldBeInvalid(string appointmentId)
    {
        // Arrange
        var dto = new CreateReviewDto(
            AppointmentId: appointmentId,
            UserId: "user123",
            VeterinarianId: "vet123",
            Rating: 5,
            Comment: "Excellent service",
            Type: ReviewType.Overall);

        // Act
        var validationResults = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(dto, new ValidationContext(dto), validationResults, true);

        // Assert
        isValid.Should().BeFalse();
        validationResults.Should().Contain(v => v.MemberNames.Contains("AppointmentId"));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(6)]
    public void CreateReviewDto_WithInvalidRating_ShouldBeInvalid(int rating)
    {
        // Arrange
        var dto = new CreateReviewDto(
            AppointmentId: "appointment123",
            UserId: "user123",
            VeterinarianId: "vet123",
            Rating: rating,
            Comment: "Excellent service",
            Type: ReviewType.Overall);

        // Act
        var validationResults = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(dto, new ValidationContext(dto), validationResults, true);

        // Assert
        isValid.Should().BeFalse();
        validationResults.Should().Contain(v => v.MemberNames.Contains("Rating"));
    }

    [Fact]
    public void UpdateReviewDto_WithValidData_ShouldBeValid()
    {
        // Arrange
        var dto = new UpdateReviewDto(
            Rating: 4,
            Comment: "Updated comment",
            Type: ReviewType.ProfessionalSkill);

        // Act
        var validationResults = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(dto, new ValidationContext(dto), validationResults, true);

        // Assert
        isValid.Should().BeTrue();
        validationResults.Should().BeEmpty();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(6)]
    public void UpdateReviewDto_WithInvalidRating_ShouldBeInvalid(int rating)
    {
        // Arrange
        var dto = new UpdateReviewDto(
            Rating: rating,
            Comment: "Updated comment",
            Type: ReviewType.ProfessionalSkill);

        // Act
        var validationResults = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(dto, new ValidationContext(dto), validationResults, true);

        // Assert
        isValid.Should().BeFalse();
        validationResults.Should().Contain(v => v.MemberNames.Contains("Rating"));
    }

    [Fact]
    public void UpdateReviewDto_WithNullValues_ShouldBeValid()
    {
        // Arrange
        var dto = new UpdateReviewDto();

        // Act
        var validationResults = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(dto, new ValidationContext(dto), validationResults, true);

        // Assert
        isValid.Should().BeTrue();
        validationResults.Should().BeEmpty();
    }
}