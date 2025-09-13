using VetUberApp.Application.DTOs;
using VetUberApp.Domain.Enums;
using Xunit;

namespace VetUberApp.IntegrationTests;

public class ReviewServiceTests : IClassFixture<TestDatabaseFixture>
{
    private readonly TestDatabaseFixture _fixture;

    public ReviewServiceTests(TestDatabaseFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task CreateReview_ShouldCreateAndRetrieveReview()
    {
        // Arrange
        var createReviewDto = new CreateReviewDto(
            AppointmentId: TestDatabaseFixture.TestAppointmentId,
            UserId: TestDatabaseFixture.TestUserId,
            VeterinarianId: TestDatabaseFixture.TestVetId,
            Rating: 5,
            Comment: "Excellent service!",
            Type: ReviewType.Overall
        );

        // Act
        var createdReview = await _fixture.ReviewService.CreateAsync(createReviewDto);
        var retrievedReview = await _fixture.ReviewService.GetByIdAsync(createdReview.Id);

        // Assert
        Assert.NotNull(retrievedReview);
        Assert.Equal(createReviewDto.UserId, retrievedReview.User.Id);
        Assert.Equal(createReviewDto.VeterinarianId, retrievedReview.Veterinarian.Id);
        Assert.Equal(createReviewDto.Rating, retrievedReview.Rating);
        Assert.Equal(createReviewDto.Comment, retrievedReview.Comment);
    }

    [Fact]
    public async Task GetReviewsByVeterinarianId_ShouldReturnAllVetReviews()
    {
        // Arrange
        var vetId = TestDatabaseFixture.TestVetId;
        var reviewDtos = new[]
        {
            new CreateReviewDto(
                AppointmentId: TestDatabaseFixture.TestAppointmentId,
                UserId: TestDatabaseFixture.TestUserId,
                VeterinarianId: vetId,
                Rating: 4,
                Comment: "Good service",
                Type: ReviewType.Overall
            ),
            new CreateReviewDto(
                AppointmentId: TestDatabaseFixture.TestAppointmentId2,
                UserId: TestDatabaseFixture.TestUser2Id,
                VeterinarianId: vetId,
                Rating: 5,
                Comment: "Excellent",
                Type: ReviewType.Overall
            )
        };

        foreach (var dto in reviewDtos)
        {
            await _fixture.ReviewService.CreateAsync(dto);
        }

        // Act
        var retrievedReviews = await _fixture.ReviewService.GetByVeterinarianIdAsync(vetId);

        // Assert
        Assert.NotNull(retrievedReviews);
        Assert.Equal(2, retrievedReviews.Count());
        Assert.All(retrievedReviews, r => Assert.Equal(vetId, r.Veterinarian.Id));
    }

    [Fact]
    public async Task UpdateReview_ShouldUpdateExistingReview()
    {
        // Arrange
        var createReviewDto = new CreateReviewDto(
            AppointmentId: TestDatabaseFixture.TestAppointmentId,
            UserId: TestDatabaseFixture.TestUserId,
            VeterinarianId: TestDatabaseFixture.TestVetId,
            Rating: 3,
            Comment: "Average service",
            Type: ReviewType.Overall
        );

        var createdReview = await _fixture.ReviewService.CreateAsync(createReviewDto);

        var updateReviewDto = new UpdateReviewDto(
            Rating: 4,
            Comment: "Better than I initially thought"
        );

        // Act
        var updatedReview = await _fixture.ReviewService.UpdateAsync(createdReview.Id, updateReviewDto);
        var retrievedReview = await _fixture.ReviewService.GetByIdAsync(createdReview.Id);

        // Assert
        Assert.NotNull(retrievedReview);
        Assert.Equal(4, retrievedReview.Rating);
        Assert.Equal("Better than I initially thought", retrievedReview.Comment);
    }

    [Fact]
    public async Task DeleteReview_ShouldRemoveReview()
    {
        // Arrange
        var createReviewDto = new CreateReviewDto(
            AppointmentId: "testAppointmentId",
            UserId: "testUserId",
            VeterinarianId: "testVetId",
            Rating: 5,
            Comment: "Great service",
            Type: ReviewType.Overall
        );

        var createdReview = await _fixture.ReviewService.CreateAsync(createReviewDto);

        // Act
        await _fixture.ReviewService.DeleteAsync(createdReview.Id);

        // Assert
        var deletedReview = await _fixture.ReviewService.GetByIdAsync(createdReview.Id);
        Assert.Null(deletedReview);
    }
}