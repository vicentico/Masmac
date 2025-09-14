using VetUberApp.Application.DTOs;
using VetUberApp.Domain.Enums;
using Xunit;
using FluentValidation;
using FluentAssertions;

namespace VetUberApp.IntegrationTests;

[Collection("IntegrationTests")]
public class ReviewValidationTests : IClassFixture<TestDatabaseFixture>
{
    private readonly TestDatabaseFixture _fixture;

    public ReviewValidationTests(TestDatabaseFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(Skip = "Validation tests should be moved to API layer tests")]
    public async Task CreateReview_WithInvalidRating_ShouldThrowValidationException()
    {
        // Arrange
        var createReviewDto = new CreateReviewDto(
            AppointmentId: TestDatabaseFixture.TestAppointmentId,
            UserId: TestDatabaseFixture.TestUserId,
            VeterinarianId: TestDatabaseFixture.TestVetId,
            Rating: 6, // Rating inválido (debe ser entre 1 y 5)
            Comment: "Great service",
            Type: ReviewType.Overall
        );

        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(() => 
            _fixture.ReviewService.CreateAsync(createReviewDto));
    }

    [Fact(Skip = "Validation tests should be moved to API layer tests")]
    public async Task CreateReview_WithInvalidComment_ShouldThrowValidationException()
    {
        // Arrange
        var createReviewDto = new CreateReviewDto(
            AppointmentId: TestDatabaseFixture.TestAppointmentId,
            UserId: TestDatabaseFixture.TestUserId,
            VeterinarianId: TestDatabaseFixture.TestVetId,
            Rating: 4,
            Comment: "Short", // Comentario muy corto (mínimo 10 caracteres)
            Type: ReviewType.Overall
        );

        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(() => 
            _fixture.ReviewService.CreateAsync(createReviewDto));
    }

    [Fact(Skip = "Validation tests should be moved to API layer tests")]
    public async Task UpdateReview_WithInvalidData_ShouldThrowValidationException()
    {
        // Arrange
        var createReviewDto = new CreateReviewDto(
            AppointmentId: TestDatabaseFixture.TestAppointmentId,
            UserId: TestDatabaseFixture.TestUserId,
            VeterinarianId: TestDatabaseFixture.TestVetId,
            Rating: 4,
            Comment: "Initial valid comment for the service",
            Type: ReviewType.Overall
        );

        var createdReview = await _fixture.ReviewService.CreateAsync(createReviewDto);

        var updateReviewDto = new UpdateReviewDto(
            Rating: 0, // Rating inválido
            Comment: "Too short" // Comentario muy corto
        );

        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(() => 
            _fixture.ReviewService.UpdateAsync(createdReview.Id, updateReviewDto));
    }

    [Fact]
    public async Task CreateReview_WithDuplicateAppointment_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var createReviewDto = new CreateReviewDto(
            AppointmentId: TestDatabaseFixture.TestAppointmentId,
            UserId: TestDatabaseFixture.TestUserId,
            VeterinarianId: TestDatabaseFixture.TestVetId,
            Rating: 4,
            Comment: "First review for this appointment",
            Type: ReviewType.Overall
        );

        // Create first review
        await _fixture.ReviewService.CreateAsync(createReviewDto);

        // Try to create another review for the same appointment
        var duplicateReviewDto = createReviewDto with { Comment = "Second review for same appointment" };

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => 
            _fixture.ReviewService.CreateAsync(duplicateReviewDto));
    }

    [Fact]
    public async Task CreateReview_WithNonexistentAppointment_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var createReviewDto = new CreateReviewDto(
            AppointmentId: "nonexistentAppointmentId",
            UserId: TestDatabaseFixture.TestUserId,
            VeterinarianId: TestDatabaseFixture.TestVetId,
            Rating: 4,
            Comment: "Review for nonexistent appointment",
            Type: ReviewType.Overall
        );

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => 
            _fixture.ReviewService.CreateAsync(createReviewDto));
    }

    [Fact]
    public async Task UpdateReview_WithNonexistentId_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var updateReviewDto = new UpdateReviewDto(
            Rating: 4,
            Comment: "Update for nonexistent review"
        );

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => 
            _fixture.ReviewService.UpdateAsync("nonexistentReviewId", updateReviewDto));
    }

    [Fact]
    public async Task CreateReview_WithUnauthorizedUser_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var createReviewDto = new CreateReviewDto(
            AppointmentId: TestDatabaseFixture.TestAppointmentId,
            UserId: TestDatabaseFixture.TestUser2Id, // Usuario diferente al de la cita
            VeterinarianId: TestDatabaseFixture.TestVetId,
            Rating: 4,
            Comment: "Attempting unauthorized review",
            Type: ReviewType.Overall
        );

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => 
            _fixture.ReviewService.CreateAsync(createReviewDto));
    }
}