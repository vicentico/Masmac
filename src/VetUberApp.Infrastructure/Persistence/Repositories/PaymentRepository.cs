using MongoDB.Driver;
using VetUberApp.Domain.Entities;
using VetUberApp.Domain.Interfaces;
using VetUberApp.Infrastructure.Persistence;

namespace VetUberApp.Infrastructure.Persistence.Repositories;

public class PaymentRepository : BaseRepository<Payment>, IPaymentRepository
{
    private readonly IMongoCollection<Payment> _payments;

    public PaymentRepository(MongoDbContext context) : base(context, "payments")
    {
        _payments = context.GetCollection<Payment>("payments");
    }

    public async Task<Payment?> GetByAppointmentIdAsync(string appointmentId)
    {
        return await _payments.Find(p => p.AppointmentId == appointmentId).FirstOrDefaultAsync();
    }
}