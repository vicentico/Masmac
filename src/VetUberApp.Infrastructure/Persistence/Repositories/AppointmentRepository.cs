using MongoDB.Driver;
using VetUberApp.Domain.Entities;
using VetUberApp.Domain.Enums;
using VetUberApp.Domain.Interfaces;

namespace VetUberApp.Infrastructure.Persistence.Repositories;

public class AppointmentRepository : BaseRepository<Appointment>, IAppointmentRepository
{
    public AppointmentRepository(MongoDbContext context) : base(context, "Appointments")
    {
    }

    public async Task<IEnumerable<Appointment>> GetByVeterinarianIdAsync(string veterinarianId)
    {
        return await Collection
            .Find(x => x.VeterinarianId == veterinarianId && !x.IsDeleted)
            .ToListAsync();
    }

    public async Task<IEnumerable<Appointment>> GetByOwnerIdAsync(string ownerId)
    {
        return await Collection
            .Find(x => x.OwnerId == ownerId && !x.IsDeleted)
            .ToListAsync();
    }

    public async Task<IEnumerable<Appointment>> GetByPetIdAsync(string petId)
    {
        return await Collection
            .Find(x => x.PetId == petId && !x.IsDeleted)
            .ToListAsync();
    }

    public async Task<IEnumerable<Appointment>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        return await Collection
            .Find(x => x.ScheduledDateTime >= startDate && 
                      x.ScheduledDateTime <= endDate && 
                      !x.IsDeleted)
            .ToListAsync();
    }

    public async Task<IEnumerable<Appointment>> GetByStatusAsync(AppointmentStatus status)
    {
        return await Collection
            .Find(x => x.Status == status && !x.IsDeleted)
            .ToListAsync();
    }

    public async Task<IEnumerable<Appointment>> GetByTypeAsync(AppointmentType type)
    {
        return await Collection
            .Find(x => x.Type == type && !x.IsDeleted)
            .ToListAsync();
    }

    public async Task<bool> IsVeterinarianAvailableAsync(string veterinarianId, DateTime dateTime)
    {
        // Buscar citas que se sobrepongan con la fecha y hora propuesta
        // Consideramos un margen de 1 hora antes y después
        var startWindow = dateTime.AddHours(-1);
        var endWindow = dateTime.AddHours(1);

        var existingAppointments = await Collection
            .CountDocumentsAsync(x => 
                x.VeterinarianId == veterinarianId &&
                x.ScheduledDateTime >= startWindow &&
                x.ScheduledDateTime <= endWindow &&
                x.Status != AppointmentStatus.Cancelled &&
                x.Status != AppointmentStatus.Completed &&
                !x.IsDeleted);

        return existingAppointments == 0;
    }
}