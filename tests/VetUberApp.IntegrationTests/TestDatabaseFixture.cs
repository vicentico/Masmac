using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using MongoDB.Bson;
using VetUberApp.Domain.Interfaces;
using VetUberApp.Application.Interfaces;
using VetUberApp.Infrastructure;
using VetUberApp.Infrastructure.Persistence;
using VetUberApp.Infrastructure.Persistence.Settings;
using VetUberApp.Domain.Entities;
using VetUberApp.Domain.Enums;
using VetUberApp.Application.Services;

namespace VetUberApp.IntegrationTests;

public class TestDatabaseFixture : IDisposable
{
    public IServiceProvider ServiceProvider { get; }
    public IUserRepository UserRepository => ServiceProvider.GetRequiredService<IUserRepository>();
    public IVeterinarianRepository VeterinarianRepository => ServiceProvider.GetRequiredService<IVeterinarianRepository>();
    public IAppointmentRepository AppointmentRepository => ServiceProvider.GetRequiredService<IAppointmentRepository>();
    public IReviewRepository ReviewRepository => ServiceProvider.GetRequiredService<IReviewRepository>();
    public IReviewService ReviewService => ServiceProvider.GetRequiredService<IReviewService>();
    public IPetRepository PetRepository => ServiceProvider.GetRequiredService<IPetRepository>();

    // Test data IDs
    public static string TestUserId { get; private set; } = string.Empty;
    public static string TestUser2Id { get; private set; } = string.Empty;
    public static string TestUser3Id { get; private set; } = string.Empty;
    public static string TestVetId { get; private set; } = string.Empty;
    public static string TestPetId { get; private set; } = string.Empty;
    public static string TestAppointmentId { get; private set; } = string.Empty;
    public static string TestAppointmentId2 { get; private set; } = string.Empty;
    public static string TestAppointmentId3 { get; private set; } = string.Empty;

    private static void EnsureIdIsNotEmpty(string id, string entityType)
    {
        if (string.IsNullOrEmpty(id))
        {
            throw new InvalidOperationException($"El ID de {entityType} no puede estar vacío");
        }
    }

    private static string GetMongoConnectionString()
    {
        // 1. Variable de entorno explícita (para CI/CD)
        var envConnectionString = Environment.GetEnvironmentVariable("MongoDb__ConnectionString");
        if (!string.IsNullOrEmpty(envConnectionString))
        {
            Console.WriteLine("Usando conexión MongoDB desde variable de entorno");
            return envConnectionString;
        }

        // 2. Detectar si estamos en CI o si MongoDB local está disponible
        if (IsRunningInCI())
        {
            Console.WriteLine("Detectado entorno CI - usando MongoDB local");
            return "mongodb://localhost:27017";
        }

        if (IsLocalMongoAvailable())
        {
            Console.WriteLine("Detectada instancia local de MongoDB");
            return "mongodb://localhost:27017";
        }

        // 3. Fallback a MongoDB Atlas para desarrollo (pero con manejo de errores)
        Console.WriteLine("Intentando usar MongoDB Atlas para desarrollo");
        return "mongodb+srv://integracion:123123123@cluster0.nbjwfob.mongodb.net/?retryWrites=true&w=majority&authSource=admin";
    }

    private static string GetDatabaseName()
    {
        var envDatabaseName = Environment.GetEnvironmentVariable("MongoDb__DatabaseName");
        if (!string.IsNullOrEmpty(envDatabaseName))
        {
            return envDatabaseName;
        }

        return IsRunningInCI() ? "VetUberAppTest" : "VetUberTestDb";
    }

    private static bool IsRunningInCI()
    {
        // Detectar si estamos ejecutando en un entorno de CI
        return !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("GITHUB_ACTIONS")) ||
               !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("CI")) ||
               !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("BUILD_NUMBER"));
    }

    private static bool IsLocalMongoAvailable()
    {
        try
        {
            var client = new MongoClient("mongodb://localhost:27017");
            var database = client.GetDatabase("test");
            database.RunCommand<BsonDocument>(new BsonDocument { { "ping", 1 } });
            return true;
        }
        catch
        {
            return false;
        }
    }

    public TestDatabaseFixture()
    {
        // Determinar la conexión según el entorno
        var connectionString = GetMongoConnectionString();
        var databaseName = GetDatabaseName();
        
        Console.WriteLine($"Configurando conexión a MongoDB: {connectionString}");
        Console.WriteLine($"Base de datos: {databaseName}");
        
        var initialData = new List<KeyValuePair<string, string?>>
        {
            new("MongoDb:ConnectionString", connectionString),
            new("MongoDb:DatabaseName", databaseName)
        };

        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(initialData)
            .Build();

        var services = new ServiceCollection();
        services.AddInfrastructure(configuration);
        services.AddScoped<IReviewService, ReviewService>();

        ServiceProvider = services.BuildServiceProvider();

        try
        {
            // Validar la conexión a la base de datos
            ValidateDatabaseConnection().Wait();

            // Limpiar y preparar la base de datos de pruebas
            CleanDatabase().Wait();
            SeedTestData().Wait();
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Error al inicializar la base de datos de pruebas: {ex.Message}\nStack Trace: {ex.StackTrace}", ex);
        }
    }

    private async Task ValidateDatabaseConnection()
    {
        var context = ServiceProvider.GetRequiredService<MongoDbContext>();
        try
        {
            Console.WriteLine("Intentando conectar a MongoDB...");
            var database = context.GetDatabase();
            Console.WriteLine("Base de datos obtenida. Verificando conexión con ping...");
            
            // Intenta una operación simple para verificar la conexión
            await database.RunCommandAsync<BsonDocument>(new BsonDocument { { "ping", 1 } });
            Console.WriteLine("Ping exitoso. Conexión a MongoDB establecida correctamente.");

            // Listar todas las bases de datos disponibles
            var client = database.Client;
            var dbList = await client.ListDatabasesAsync();
            var dbNames = await dbList.ToListAsync();
            Console.WriteLine("\nBases de datos disponibles:");
            foreach (var db in dbNames)
            {
                Console.WriteLine($"- {db["name"]}");
            }

            // Verificar si las colecciones existen, si no, crearlas
            Console.WriteLine($"\nVerificando colecciones en la base de datos actual...");
            var cursor = await database.ListCollectionNamesAsync();
            var collections = await cursor.ToListAsync();
            var requiredCollections = new[] { "Users", "Veterinarians", "Appointments", "Reviews", "Pets" };

            foreach (var collection in requiredCollections)
            {
                if (!collections.Contains(collection))
                {
                    Console.WriteLine($"Creando colección {collection}...");
                    await database.CreateCollectionAsync(collection);
                    Console.WriteLine($"Colección {collection} creada exitosamente.");
                }
                else
                {
                    Console.WriteLine($"Colección {collection} ya existe.");
                }
            }
        }
        catch (MongoException mongoEx)
        {
            var errorMessage = $"Error de MongoDB: {mongoEx.Message}";
            Console.WriteLine($"ERROR: {errorMessage}\nStack Trace: {mongoEx.StackTrace}");
            throw new InvalidOperationException(errorMessage, mongoEx);
        }
        catch (Exception ex)
        {
            var errorMessage = $"Error al conectar con MongoDB: {ex.Message}";
            Console.WriteLine($"ERROR: {errorMessage}\nStack Trace: {ex.StackTrace}");
            throw new InvalidOperationException(errorMessage, ex);
        }
    }

    private async Task SeedTestData()
    {
        try
        {
            Console.WriteLine("Iniciando la creación de datos de prueba...");

            // Create test users
            Console.WriteLine("Creando usuarios de prueba...");
            var users = new[]
            {
                new User
                {
                    FirstName = "Test",
                    LastName = "User",
                    Email = "test@example.com",
                    PhoneNumber = "1234567890"
                },
                new User
                {
                    FirstName = "Test",
                    LastName = "User1",
                    Email = "test1@example.com",
                    PhoneNumber = "1234567891"
                },
                new User
                {
                    FirstName = "Test",
                    LastName = "User2",
                    Email = "test2@example.com",
                    PhoneNumber = "1234567892"
                }
            };

            // Crear usuarios y guardar sus IDs
            var createdUsers = new List<User>();
            foreach (var user in users)
            {
                var createdUser = await UserRepository.CreateAsync(user);
                if (string.IsNullOrEmpty(createdUser.Id))
                    throw new InvalidOperationException($"Error al crear usuario: No se generó ID");
                createdUsers.Add(createdUser);
                Console.WriteLine($"Usuario creado: {createdUser.Id}");
            }

            // Asignar IDs y validar
            TestUserId = createdUsers[0].Id ?? throw new InvalidOperationException("ID de usuario 1 es nulo");
            TestUser2Id = createdUsers[1].Id ?? throw new InvalidOperationException("ID de usuario 2 es nulo");
            TestUser3Id = createdUsers[2].Id ?? throw new InvalidOperationException("ID de usuario 3 es nulo");
            
            // Validar que los IDs se hayan asignado correctamente
            EnsureIdIsNotEmpty(TestUserId, "usuario 1");
            EnsureIdIsNotEmpty(TestUser2Id, "usuario 2");
            EnsureIdIsNotEmpty(TestUser3Id, "usuario 3");

            // Create test veterinarian
            Console.WriteLine("Creando veterinario de prueba...");
            var veterinarian = new Veterinarian
            {
                FirstName = "Test",
                LastName = "Vet",
                Email = "testvet@example.com",
                PhoneNumber = "0987654321",
                LicenseNumber = "VET123",
                Specialties = new List<string> { "General" },
                PasswordHash = "hashedPassword123",
                CurrentLocation = new Domain.ValueObjects.Address
                {
                    Street = "Test Street",
                    City = "Test City",
                    State = "Test State",
                    Country = "Test Country",
                    ZipCode = "12345",
                    Latitude = 0,
                    Longitude = 0
                },
                IsAvailable = true,
                ConsultationFee = 50.00m,
                Rating = 0m,
                IsVerified = true,
                Reviews = new List<Review>(),
                Appointments = new List<Appointment>()
            };
            var createdVet = await VeterinarianRepository.CreateAsync(veterinarian);
            if (string.IsNullOrEmpty(createdVet.Id))
                throw new InvalidOperationException("Error al crear veterinario de prueba - No se generó un ID");
                
            // Guardamos el ID generado en la constante TestVetId
            TestVetId = createdVet.Id;
            Console.WriteLine($"Veterinario creado exitosamente con ID: {TestVetId}");

            // Create test pet
            Console.WriteLine("Creando mascota de prueba...");
            var pet = new Pet
            {
                Name = "TestPet",
                Type = PetType.Dog,
                Breed = "Mixed",
                DateOfBirth = DateTime.Now.AddYears(-2),
                OwnerId = TestUserId
            };
            var createdPet = await PetRepository.CreateAsync(pet);
            if (string.IsNullOrEmpty(createdPet.Id))
                throw new InvalidOperationException("Error al crear mascota de prueba");
            TestPetId = createdPet.Id;
            Console.WriteLine($"Mascota creada exitosamente con ID: {TestPetId}");

            // Create test appointments
            Console.WriteLine("Creando citas de prueba...");
            var appointmentsToCreate = new[]
            {
                new Appointment
                {
                    PetId = TestPetId,
                    VeterinarianId = TestVetId,
                    OwnerId = TestUserId,
                    ScheduledDateTime = DateTime.Now.AddDays(-1), // Cita en el pasado
                    Status = AppointmentStatus.Completed // Estado completado para poder dejar reseña
                },
                new Appointment
                {
                    PetId = TestPetId,
                    VeterinarianId = TestVetId,
                    OwnerId = TestUser2Id,
                    ScheduledDateTime = DateTime.Now.AddDays(-2),
                    Status = AppointmentStatus.Completed
                },
                new Appointment
                {
                    PetId = TestPetId,
                    VeterinarianId = TestVetId,
                    OwnerId = TestUser3Id,
                    ScheduledDateTime = DateTime.Now.AddDays(-3),
                    Status = AppointmentStatus.Completed
                }
            };

            var createdAppointments = new List<Appointment>();
            foreach (var appointment in appointmentsToCreate)
            {
                var createdAppointment = await AppointmentRepository.CreateAsync(appointment);
                if (string.IsNullOrEmpty(createdAppointment.Id))
                    throw new InvalidOperationException("Error al crear cita - No se generó un ID");
                createdAppointments.Add(createdAppointment);
                Console.WriteLine($"Cita creada: {createdAppointment.Id}");
            }

            // Asignar los IDs generados y validar
            TestAppointmentId = createdAppointments[0].Id ?? throw new InvalidOperationException("ID de cita 1 es nulo");
            TestAppointmentId2 = createdAppointments[1].Id ?? throw new InvalidOperationException("ID de cita 2 es nulo");
            TestAppointmentId3 = createdAppointments[2].Id ?? throw new InvalidOperationException("ID de cita 3 es nulo");

            // Validar que los IDs se hayan asignado correctamente
            EnsureIdIsNotEmpty(TestAppointmentId, "cita 1");
            EnsureIdIsNotEmpty(TestAppointmentId2, "cita 2");
            EnsureIdIsNotEmpty(TestAppointmentId3, "cita 3");

            Console.WriteLine("Datos de prueba creados exitosamente");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al crear datos de prueba: {ex.Message}\nStack Trace: {ex.StackTrace}");
            throw new InvalidOperationException("Error al crear datos de prueba", ex);
        }
    }

    private async Task CleanDatabase()
    {
        var context = ServiceProvider.GetRequiredService<MongoDbContext>();
        var database = context.GetDatabase();
        await database.DropCollectionAsync("Users");
        await database.DropCollectionAsync("Veterinarians");
        await database.DropCollectionAsync("Appointments");
        await database.DropCollectionAsync("Reviews");
        await database.DropCollectionAsync("Pets");
    }

    public void Dispose()
    {
       // CleanDatabase().Wait();
    }
}