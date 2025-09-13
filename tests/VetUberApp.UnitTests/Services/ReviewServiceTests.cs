using Moq;
using VetUberApp.Application.DTOs;
using VetUberApp.Application.Services;
using VetUberApp.Domain.Entities;
using VetUberApp.Domain.Enums;
using VetUberApp.Domain.Interfaces;
using FluentAssertions;

namespace VetUberApp.UnitTests.Services;

public class ReviewServiceTests
{
    private readonly Mock<IReviewRepository> _mockReviewRepository;
    private readonly Mock<IAppointmentRepository> _mockAppointmentRepository;
    private readonly Mock<IUserRepository> _mockUserRepository;
    private readonly Mock<IVeterinarianRepository> _mockVeterinarianRepository;
    private readonly ReviewService _service;

    public ReviewServiceTests()
    {
        _mockReviewRepository = new Mock<IReviewRepository>();
        _mockAppointmentRepository = new Mock<IAppointmentRepository>();
        _mockUserRepository = new Mock<IUserRepository>();
        _mockVeterinarianRepository = new Mock<IVeterinarianRepository>();
        var mockPetRepository = new Mock<IPetRepository>();

        _service = new ReviewService(
            _mockReviewRepository.Object,
            _mockAppointmentRepository.Object,
            _mockUserRepository.Object,
            _mockVeterinarianRepository.Object,
            mockPetRepository.Object);
    }

    [Fact]
    public async Task CreateAsync_WithValidData_ShouldCreateReview()
    {
        // Arrange
        var dto = new CreateReviewDto(
            AppointmentId: "appointment123",
            UserId: "user123",
            VeterinarianId: "vet123",
            Rating: 5,
            Comment: "Excellent service",
            Type: ReviewType.Overall);

        var appointment = new Appointment
        {
            Id = dto.AppointmentId,
            OwnerId = dto.UserId,
            VeterinarianId = dto.VeterinarianId
        };

        _mockAppointmentRepository.Setup(r => r.GetByIdAsync(dto.AppointmentId))
            .ReturnsAsync(appointment);

        _mockReviewRepository.Setup(r => r.HasUserReviewedAppointmentAsync(dto.UserId, dto.AppointmentId))
            .ReturnsAsync(false);

        _mockReviewRepository.Setup(r => r.CreateAsync(It.IsAny<Review>()))
            .ReturnsAsync((Review r) => r);

        // Act
        var result = await _service.CreateAsync(dto);

        // Assert
        result.Should().NotBeNull();
        result.Rating.Should().Be(dto.Rating);
        result.Comment.Should().Be(dto.Comment);
        result.Type.Should().Be(dto.Type);
    }

    [Fact]
    public async Task CreateAsync_WhenAppointmentNotFound_ShouldThrow()
    {
        // Arrange
        var dto = new CreateReviewDto(
            AppointmentId: "appointment123",
            UserId: "user123",
            VeterinarianId: "vet123",
            Rating: 5,
            Comment: "Excellent service",
            Type: ReviewType.Overall);

        _mockAppointmentRepository.Setup(r => r.GetByIdAsync(dto.AppointmentId))
            .ReturnsAsync((Appointment?)null);

        // Act
        var action = () => _service.CreateAsync(dto);

        // Assert
        await action.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("La cita especificada no existe.");
    }

    [Fact]
    public async Task CreateAsync_WhenUserNotOwner_ShouldThrow()
    {
        // Arrange
        var dto = new CreateReviewDto(
            AppointmentId: "appointment123",
            UserId: "user123",
            VeterinarianId: "vet123",
            Rating: 5,
            Comment: "Excellent service",
            Type: ReviewType.Overall);

        var appointment = new Appointment
        {
            Id = dto.AppointmentId,
            OwnerId = "differentUser",
            VeterinarianId = dto.VeterinarianId
        };

        _mockAppointmentRepository.Setup(r => r.GetByIdAsync(dto.AppointmentId))
            .ReturnsAsync(appointment);

        // Act
        var action = () => _service.CreateAsync(dto);

        // Assert
        await action.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("El usuario no está autorizado para crear una reseña para esta cita.");
    }

    [Fact]
    public async Task CreateAsync_WhenReviewExists_ShouldThrow()
    {
        // Arrange
        var dto = new CreateReviewDto(
            AppointmentId: "appointment123",
            UserId: "user123",
            VeterinarianId: "vet123",
            Rating: 5,
            Comment: "Excellent service",
            Type: ReviewType.Overall);

        var appointment = new Appointment
        {
            Id = dto.AppointmentId,
            OwnerId = dto.UserId,
            VeterinarianId = dto.VeterinarianId
        };

        _mockAppointmentRepository.Setup(r => r.GetByIdAsync(dto.AppointmentId))
            .ReturnsAsync(appointment);

        _mockReviewRepository.Setup(r => r.HasUserReviewedAppointmentAsync(dto.UserId, dto.AppointmentId))
            .ReturnsAsync(true);

        // Act
        var action = () => _service.CreateAsync(dto);

        // Assert
        await action.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("El usuario ya ha creado una reseña para esta cita.");
    }

    [Fact]
    public async Task UpdateAsync_WithValidData_ShouldUpdateReview()
    {
        // Arrange
        var reviewId = "review123";
        var dto = new UpdateReviewDto(
            Rating: 4,
            Comment: "Updated comment",
            Type: ReviewType.ProfessionalSkill);

        var existingReview = new Review
        {
            Id = reviewId,
            Rating = 5,
            Comment = "Original comment",
            Type = ReviewType.Overall
        };

        _mockReviewRepository.Setup(r => r.GetByIdAsync(reviewId))
            .ReturnsAsync(existingReview);

        _mockReviewRepository.Setup(r => r.UpdateAsync(It.IsAny<Review>()))
            .ReturnsAsync((Review r) => r);

        // Act
        var result = await _service.UpdateAsync(reviewId, dto);

        // Assert
        result.Should().NotBeNull();
        result.Rating.Should().Be(dto.Rating);
        result.Comment.Should().Be(dto.Comment);
        result.Type.Should().Be(dto.Type!.Value);
    }

    [Fact]
    public async Task GetVeterinarianAverageRatingAsync_ShouldReturnAverage()
    {
        // Arrange
        var veterinarianId = "vet123";
        var expectedAverage = 4.5m;

        _mockReviewRepository.Setup(r => r.GetAverageRatingByVeterinarianIdAsync(veterinarianId))
            .ReturnsAsync(expectedAverage);

        // Act
        var result = await _service.GetVeterinarianAverageRatingAsync(veterinarianId);

        // Assert
        result.Should().Be(expectedAverage);
    }
}