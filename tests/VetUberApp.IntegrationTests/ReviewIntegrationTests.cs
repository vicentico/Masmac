using Microsoft.Extensions.DependencyInjection;
using VetUberApp.Application.DTOs;
using VetUberApp.Application.Services;
using VetUberApp.Domain.Entities;
using VetUberApp.Domain.Enums;
using VetUberApp.Infrastructure.Persistence;
using FluentAssertions;

namespace VetUberApp.IntegrationTests;

public class ReviewIntegrationTests : IClassFixture<TestDatabaseFixture>
{
    private readonly ReviewService _reviewService;
    private readonly TestDatabaseFixture _fixture;

    public ReviewIntegrationTests(TestDatabaseFixture fixture)
    {
        _fixture = fixture;
        _reviewService = _fixture.ServiceProvider.GetRequiredService<ReviewService>();
    }

    [Fact]
    public async Task CreateAndRetrieveReview_ShouldSucceed()
    {
        // Arrange
        var user = await CreateTestUser();
        var veterinarian = await CreateTestVeterinarian();
        var appointment = await CreateTestAppointment(user.Id, veterinarian.Id);

        var createDto = new CreateReviewDto(
            AppointmentId: appointment.Id,
            UserId: user.Id,
            VeterinarianId: veterinarian.Id,
            Rating: 5,
            Comment: "Excellent service",
            Type: ReviewType.Overall);

        // Act
        var createdReview = await _reviewService.CreateAsync(createDto);
        var retrievedReview = await _reviewService.GetByIdAsync(createdReview.Id);

        // Assert
        retrievedReview.Should().NotBeNull();
        retrievedReview!.Rating.Should().Be(createDto.Rating);
        retrievedReview.Comment.Should().Be(createDto.Comment);
        retrievedReview.Type.Should().Be(createDto.Type);
    }

    [Fact]
    public async Task UpdateReview_ShouldReflectChanges()
    {
        // Arrange
        var user = await CreateTestUser();
        var veterinarian = await CreateTestVeterinarian();
        var appointment = await CreateTestAppointment(user.Id, veterinarian.Id);

        var createDto = new CreateReviewDto(
            AppointmentId: appointment.Id,
            UserId: user.Id,
            VeterinarianId: veterinarian.Id,
            Rating: 5,
            Comment: "Original comment",
            Type: ReviewType.Overall);

        var createdReview = await _reviewService.CreateAsync(createDto);

        var updateDto = new UpdateReviewDto(
            Rating: 4,
            Comment: "Updated comment",
            Type: ReviewType.ProfessionalSkill);

        // Act
        var updatedReview = await _reviewService.UpdateAsync(createdReview.Id, updateDto);
        var retrievedReview = await _reviewService.GetByIdAsync(createdReview.Id);

        // Assert
        retrievedReview.Should().NotBeNull();
        retrievedReview!.Rating.Should().Be(updateDto.Rating!.Value);
        retrievedReview.Comment.Should().Be(updateDto.Comment);
        retrievedReview.Type.Should().Be(updateDto.Type!.Value);
    }

    [Fact]
    public async Task DeleteReview_ShouldNotBeFound()
    {
        // Arrange
        var user = await CreateTestUser();
        var veterinarian = await CreateTestVeterinarian();
        var appointment = await CreateTestAppointment(user.Id, veterinarian.Id);

        var createDto = new CreateReviewDto(
            AppointmentId: appointment.Id,
            UserId: user.Id,
            VeterinarianId: veterinarian.Id,
            Rating: 5,
            Comment: "Test comment",
            Type: ReviewType.Overall);

        var createdReview = await _reviewService.CreateAsync(createDto);

        // Act
        await _reviewService.DeleteAsync(createdReview.Id);
        var retrievedReview = await _reviewService.GetByIdAsync(createdReview.Id);

        // Assert
        retrievedReview.Should().BeNull();
    }

    [Fact]
    public async Task GetVeterinarianReviews_ShouldReturnAllReviews()
    {
        // Arrange
        var user = await CreateTestUser();
        var veterinarian = await CreateTestVeterinarian();
        var appointment1 = await CreateTestAppointment(user.Id, veterinarian.Id);
        var appointment2 = await CreateTestAppointment(user.Id, veterinarian.Id);

        var createDto1 = new CreateReviewDto(
            AppointmentId: appointment1.Id,
            UserId: user.Id,
            VeterinarianId: veterinarian.Id,
            Rating: 5,
            Comment: "First review",
            Type: ReviewType.Overall);

        var createDto2 = new CreateReviewDto(
            AppointmentId: appointment2.Id,
            UserId: user.Id,
            VeterinarianId: veterinarian.Id,
            Rating: 4,
            Comment: "Second review",
            Type: ReviewType.ProfessionalSkill);

        await _reviewService.CreateAsync(createDto1);
        await _reviewService.CreateAsync(createDto2);

        // Act
        var reviews = await _reviewService.GetByVeterinarianIdAsync(veterinarian.Id);

        // Assert
        reviews.Should().HaveCount(2);
        reviews.Should().Contain(r => r.Comment == "First review");
        reviews.Should().Contain(r => r.Comment == "Second review");
    }

    private async Task<User> CreateTestUser()
    {
        var user = new User
        {
            Email = $"test{Guid.NewGuid()}@example.com",
            PasswordHash = "hashedPassword",
            FirstName = "Test",
            LastName = "User",
            PhoneNumber = "1234567890",
            Role = UserRole.Client,
            IsVerified = true
        };

        return await _fixture.UserRepository.CreateAsync(user);
    }

    private async Task<Veterinarian> CreateTestVeterinarian()
    {
        var veterinarian = new Veterinarian
        {
            Email = $"vet{Guid.NewGuid()}@example.com",
            PasswordHash = "hashedPassword",
            FirstName = "Test",
            LastName = "Veterinarian",
            PhoneNumber = "1234567890",
            LicenseNumber = "LICENSE123",
            IsVerified = true,
            IsAvailable = true
        };

        return await _fixture.VeterinarianRepository.CreateAsync(veterinarian);
    }

    private async Task<Appointment> CreateTestAppointment(string userId, string veterinarianId)
    {
        var appointment = new Appointment
        {
            OwnerId = userId,
            VeterinarianId = veterinarianId,
            ScheduledDateTime = DateTime.UtcNow.AddDays(1),
            Status = AppointmentStatus.Completed
        };

        return await _fixture.AppointmentRepository.CreateAsync(appointment);
    }
}