using VetUberApp.Domain.Entities;

namespace VetUberApp.Domain.Interfaces;

public interface IPaymentRepository : IBaseRepository<Payment>
{
    Task<Payment?> GetByAppointmentIdAsync(string appointmentId);
}