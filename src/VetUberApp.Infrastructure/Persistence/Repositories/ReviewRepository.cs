using MongoDB.Driver;
using VetUberApp.Domain.Entities;
using VetUberApp.Domain.Enums;
using VetUberApp.Domain.Interfaces;

namespace VetUberApp.Infrastructure.Persistence.Repositories;

public class ReviewRepository : BaseRepository<Review>, IReviewRepository
{
    public ReviewRepository(MongoDbContext context) : base(context, "Reviews")
    {
    }

    public async Task<IEnumerable<Review>> GetReviewsByVeterinarianId(string veterinarianId)
    {
        return await Collection
            .Find(x => x.VeterinarianId == veterinarianId && !x.IsDeleted)
            .ToListAsync();
    }

    public async Task<IEnumerable<Review>> GetReviewsByUserId(string userId)
    {
        return await Collection
            .Find(x => x.UserId == userId && !x.IsDeleted)
            .ToListAsync();
    }

    public async Task<IEnumerable<Review>> GetReviewsByAppointmentId(string appointmentId)
    {
        return await Collection
            .Find(x => x.AppointmentId == appointmentId && !x.IsDeleted)
            .ToListAsync();
    }

    public async Task<IEnumerable<Review>> GetByTypeAsync(ReviewType type)
    {
        return await Collection
            .Find(x => x.Type == type && !x.IsDeleted)
            .ToListAsync();
    }

    public async Task<IEnumerable<Review>> GetByMinimumRatingAsync(int rating)
    {
        return await Collection
            .Find(x => x.Rating >= rating && !x.IsDeleted)
            .ToListAsync();
    }

    public async Task<decimal> GetAverageRatingByVeterinarianIdAsync(string veterinarianId)
    {
        var reviews = await Collection
            .Find(x => x.VeterinarianId == veterinarianId && !x.IsDeleted)
            .ToListAsync();

        if (!reviews.Any())
            return 0m;

        var average = reviews.Average(r => r.Rating);
        return decimal.Round(average, 2);
    }

    public async Task<bool> HasUserReviewedAppointmentAsync(string userId, string appointmentId)
    {
        var review = await Collection
            .Find(x => x.UserId == userId && 
                      x.AppointmentId == appointmentId && 
                      !x.IsDeleted)
            .FirstOrDefaultAsync();

        return review != null;
    }
}