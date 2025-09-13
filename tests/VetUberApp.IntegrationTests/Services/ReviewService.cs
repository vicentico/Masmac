using VetUberApp.Application.DTOs;
using VetUberApp.Application.Interfaces;
using VetUberApp.Domain.Enums;
using VetUberApp.Domain.Interfaces;

namespace VetUberApp.IntegrationTests.Services;

public class ReviewService : IReviewService
{
    private readonly IReviewRepository _reviewRepository;

    public ReviewService(IReviewRepository reviewRepository)
    {
        _reviewRepository = reviewRepository;
    }

    public async Task<ReviewDto> CreateAsync(CreateReviewDto dto)
    {
        var entity = new VetUberApp.Domain.Entities.Review
        {
            AppointmentId = dto.AppointmentId,
            UserId = dto.UserId,
            VeterinarianId = dto.VeterinarianId,
            Rating = dto.Rating,
            Comment = dto.Comment,
            Type = dto.Type
        };

        var review = await _reviewRepository.CreateAsync(entity);
        return MapToDto(review);
    }

    public async Task<ReviewDto?> GetByIdAsync(string id)
    {
        var review = await _reviewRepository.GetByIdAsync(id);
        return review != null ? MapToDto(review) : null;
    }

    public async Task<IEnumerable<ReviewDto>> GetAllAsync()
    {
        var reviews = await _reviewRepository.GetAllAsync();
        return reviews.Select(MapToDto);
    }

    public async Task<IEnumerable<ReviewDto>> GetByVeterinarianIdAsync(string veterinarianId)
    {
        var reviews = await _reviewRepository.GetReviewsByVeterinarianId(veterinarianId);
        return reviews.Select(MapToDto);
    }

    public async Task<IEnumerable<ReviewDto>> GetByUserIdAsync(string userId)
    {
        var reviews = await _reviewRepository.GetReviewsByUserId(userId);
        return reviews.Select(MapToDto);
    }

    public async Task<ReviewDto> UpdateAsync(string id, UpdateReviewDto dto)
    {
        var review = await _reviewRepository.GetByIdAsync(id)
            ?? throw new InvalidOperationException("Review not found");

        review.Rating = (int)dto.Rating;
        review.Comment = dto.Comment ?? string.Empty;

        var updatedReview = await _reviewRepository.UpdateAsync(review);
        return MapToDto(updatedReview);
    }

    public async Task DeleteAsync(string id)
    {
        await _reviewRepository.DeleteAsync(id);
    }

    private static ReviewDto MapToDto(VetUberApp.Domain.Entities.Review review)
    {
        // Aquí normalmente consultaríamos las entidades relacionadas
        // Para las pruebas, usaremos datos básicos
        var appointmentBasicDto = new AppointmentBasicDto
        {
            Id = review.AppointmentId,
            VeterinarianName = "Dr. Test",
            PetName = "Test Pet",
            ScheduledDateTime = DateTime.UtcNow,
            Status = AppointmentStatus.Completed
        };

        var userBasicDto = new UserBasicDto
        {
            Id = review.UserId,
            Name = "Test User",
            Email = "test@example.com",
            Phone = "123456789"
        };

        var veterinarianBasicDto = new VeterinarianBasicDto
        {
            Id = review.VeterinarianId,
            Name = "Dr. Test",
            Specialty = "General",
            LicenseNumber = "TEST123",
            Rating = 4.5
        };

        return new ReviewDto(
            review.Id,
            appointmentBasicDto,
            userBasicDto,
            veterinarianBasicDto,
            review.Rating,
            review.Comment,
            review.Type,
            review.CreatedAt,
            review.UpdatedAt
        );
    }
}