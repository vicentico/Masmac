using VetUberApp.Application.DTOs;

namespace VetUberApp.Application.Interfaces;

public interface IReviewService
{
    Task<ReviewDto> CreateAsync(CreateReviewDto dto);
    Task<ReviewDto?> GetByIdAsync(string id);
    Task<IEnumerable<ReviewDto>> GetAllAsync();
    Task<IEnumerable<ReviewDto>> GetByUserIdAsync(string userId);
    Task<IEnumerable<ReviewDto>> GetByVeterinarianIdAsync(string veterinarianId);
    Task<ReviewDto> UpdateAsync(string id, UpdateReviewDto dto);
    Task DeleteAsync(string id);
}