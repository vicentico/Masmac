using VetUberApp.Application.DTOs;
using VetUberApp.Domain.Entities;
using VetUberApp.Domain.Enums;
using Microsoft.Extensions.DependencyInjection;
using VetUberApp.Application.Interfaces;
using Xunit;

namespace VetUberApp.IntegrationTests;

[Collection("IntegrationTests")]
public class ReviewServiceTests : IClassFixture<TestDatabaseFixture>
{
    private readonly TestDatabaseFixture _fixture;
    private readonly IReviewService _reviewService;

    public ReviewServiceTests(TestDatabaseFixture fixture)
    {
        _fixture = fixture;
        _reviewService = _fixture.ServiceProvider.GetRequiredService<IReviewService>();
    }

    [Fact]
    public async Task CreateReview_ShouldCreateAndRetrieveReview()
    {
        // Arrange
        var user = await CreateTestUser();
        var veterinarian = await CreateTestVeterinarian();
        var appointment = await CreateTestAppointment(user.Id, veterinarian.Id);

        var createReviewDto = new CreateReviewDto(
            AppointmentId: appointment.Id,
            UserId: user.Id,
            VeterinarianId: veterinarian.Id,
            Rating: 5,
            Comment: "Excellent service!",
            Type: ReviewType.Overall
        );

        // Act
        var createdReview = await _reviewService.CreateAsync(createReviewDto);
        var retrievedReview = await _reviewService.GetByIdAsync(createdReview.Id);

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
        var user1 = await CreateTestUser();
        var user2 = await CreateTestUser();
        var veterinarian = await CreateTestVeterinarian();
        var appointment1 = await CreateTestAppointment(user1.Id, veterinarian.Id);
        var appointment2 = await CreateTestAppointment(user2.Id, veterinarian.Id);

        var reviewDtos = new[]
        {
            new CreateReviewDto(
                AppointmentId: appointment1.Id,
                UserId: user1.Id,
                VeterinarianId: veterinarian.Id,
                Rating: 4,
                Comment: "Good service",
                Type: ReviewType.Overall
            ),
            new CreateReviewDto(
                AppointmentId: appointment2.Id,
                UserId: user2.Id,
                VeterinarianId: veterinarian.Id,
                Rating: 5,
                Comment: "Excellent",
                Type: ReviewType.Overall
            )
        };

        foreach (var dto in reviewDtos)
        {
            await _reviewService.CreateAsync(dto);
        }

        // Act
        var retrievedReviews = await _reviewService.GetByVeterinarianIdAsync(veterinarian.Id);

        // Assert
        Assert.NotNull(retrievedReviews);
        Assert.Equal(2, retrievedReviews.Count());
        Assert.All(retrievedReviews, r => Assert.Equal(veterinarian.Id, r.Veterinarian.Id));
    }

    [Fact]
    public async Task UpdateReview_ShouldUpdateExistingReview()
    {
        // Arrange
        var user = await CreateTestUser();
        var veterinarian = await CreateTestVeterinarian();
        var appointment = await CreateTestAppointment(user.Id, veterinarian.Id);

        var createReviewDto = new CreateReviewDto(
            AppointmentId: appointment.Id,
            UserId: user.Id,
            VeterinarianId: veterinarian.Id,
            Rating: 3,
            Comment: "Average service",
            Type: ReviewType.Overall
        );

        var createdReview = await _reviewService.CreateAsync(createReviewDto);

        var updateReviewDto = new UpdateReviewDto(
            Rating: 4,
            Comment: "Better than I initially thought"
        );

        // Act
        var updatedReview = await _reviewService.UpdateAsync(createdReview.Id, updateReviewDto);
        var retrievedReview = await _reviewService.GetByIdAsync(createdReview.Id);

        // Assert
        Assert.NotNull(retrievedReview);
        Assert.Equal(4, retrievedReview.Rating);
        Assert.Equal("Better than I initially thought", retrievedReview.Comment);
    }

    [Fact]
    public async Task DeleteReview_ShouldRemoveReview()
    {
        // Arrange
        var user = await CreateTestUser();
        var veterinarian = await CreateTestVeterinarian();
        var appointment = await CreateTestAppointment(user.Id, veterinarian.Id);

        var createReviewDto = new CreateReviewDto(
            AppointmentId: appointment.Id,
            UserId: user.Id,
            VeterinarianId: veterinarian.Id,
            Rating: 5,
            Comment: "Great service",
            Type: ReviewType.Overall
        );

        var createdReview = await _reviewService.CreateAsync(createReviewDto);

        // Act
        await _reviewService.DeleteAsync(createdReview.Id);

        // Assert
        var deletedReview = await _reviewService.GetByIdAsync(createdReview.Id);
        Assert.Null(deletedReview);
    }

    // Helper methods for creating test entities
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
            LicenseNumber = $"LIC{Guid.NewGuid().ToString()[..8].ToUpper()}",
            IsVerified = true,
            IsAvailable = true
        };

        return await _fixture.VeterinarianRepository.CreateAsync(veterinarian);
    }

    private async Task<Pet> CreateTestPet(string ownerId)
    {
        var pet = new Pet
        {
            OwnerId = ownerId,
            Name = $"TestPet{Guid.NewGuid().ToString()[..8]}",
            Type = PetType.Dog,
            Breed = "Golden Retriever",
            DateOfBirth = DateTime.UtcNow.AddYears(-3),
            Weight = 25.5
        };

        return await _fixture.PetRepository.CreateAsync(pet);
    }

    private async Task<Appointment> CreateTestAppointment(string userId, string veterinarianId)
    {
        // Create a pet first since it's required
        var pet = await CreateTestPet(userId);

        // Small delay to ensure the pet is persisted
        await Task.Delay(100);

        var appointment = new Appointment
        {
            OwnerId = userId,
            VeterinarianId = veterinarianId,
            PetId = pet.Id,
            ScheduledDateTime = DateTime.UtcNow.AddDays(1),
            Reason = "Regular checkup",
            Type = AppointmentType.CheckUp,
            Status = AppointmentStatus.Completed
        };

        var createdAppointment = await _fixture.AppointmentRepository.CreateAsync(appointment);
        
        // Small delay to ensure the appointment is persisted
        await Task.Delay(100);
        
        return createdAppointment;
    }
}