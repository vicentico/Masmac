using VetUberApp.Domain.Entities;
using MongoDB.Driver;
using Moq;
using VetUberApp.Infrastructure.Persistence;

namespace VetUberApp.UnitTests.Mocks;

public class MockMongoDbContext 
{
    public static MongoDbContext GetMockContext()
    {
        var mockContext = new Mock<MongoDbContext>();

        // Configurar colecciones mock para cada tipo de entidad
        var mockUserCollection = new Mock<IMongoCollection<User>>();
        var mockVetCollection = new Mock<IMongoCollection<Veterinarian>>();
        var mockReviewCollection = new Mock<IMongoCollection<Review>>();
        var mockAppointmentCollection = new Mock<IMongoCollection<Appointment>>();
        var mockPetCollection = new Mock<IMongoCollection<Pet>>();

        // Configurar GetCollection para devolver la colección mock correspondiente
        mockContext.Setup(x => x.GetCollection<User>("Users"))
            .Returns(mockUserCollection.Object);
            
        mockContext.Setup(x => x.GetCollection<Veterinarian>("Veterinarians"))
            .Returns(mockVetCollection.Object);
            
        mockContext.Setup(x => x.GetCollection<Review>("Reviews"))
            .Returns(mockReviewCollection.Object);
            
        mockContext.Setup(x => x.GetCollection<Appointment>("Appointments"))
            .Returns(mockAppointmentCollection.Object);
            
        mockContext.Setup(x => x.GetCollection<Pet>("Pets"))
            .Returns(mockPetCollection.Object);

        return mockContext.Object;
    }
}