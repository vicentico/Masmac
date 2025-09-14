# VetUberApp - AI Coding Assistant Instructions

## Architecture & Project Structure

This is a **Clean Architecture** .NET 9 API for a veterinary home-service platform. The project follows strict **layered separation**:

- **API Layer** (`VetUberApp.API`): Controllers, filters, and HTTP concerns
- **Application Layer** (`VetUberApp.Application`): Services, DTOs, validators, interfaces  
- **Domain Layer** (`VetUberApp.Domain`): Entities, enums, value objects, domain interfaces
- **Infrastructure Layer** (`VetUberApp.Infrastructure`): MongoDB repositories, external integrations
- **Shared Layer** (`VetUberApp.Shared`): Cross-cutting concerns

**Key Rule**: Domain never references other layers. Infrastructure and Application reference Domain. API references Application.

## Essential Patterns & Conventions

### Entity Structure
All entities inherit from `BaseEntity` with common properties (`Id`, `CreatedAt`, `UpdatedAt`, `IsDeleted`). Example:
```csharp
public class Review : BaseEntity
{
    public string AppointmentId { get; set; } = null!;
    public ReviewType Type { get; set; }
}
```

### MongoDB Integration
- Uses **MongoDB.Driver** with `MongoDbContext` wrapper
- Collections named in plural: `Reviews`, `Appointments`, `Users`
- Connection configured via `MongoDbSettings` in DI container
- Repositories follow pattern: `IEntityRepository` → `EntityRepository`

### Validation Pattern
Uses **FluentValidation** with global `ValidationFilter`:
- DTOs automatically validated if validator exists
- Validators named: `CreateEntityDtoValidator`, `UpdateEntityDtoValidator`
- Registered in Application layer DI

### Service Layer Structure
Services implement interfaces from Application layer:
```csharp
// Interface in Application/Interfaces
public interface IReviewService
// Implementation in Application/Services  
public class ReviewService : IReviewService
```

### Controller Conventions
- Inherit from `ControllerBase`
- Route: `[Route("api/[controller]")]`
- XML documentation for Swagger
- Consistent error handling pattern:
```csharp
catch (InvalidOperationException ex)
{
    return BadRequest(new { message = ex.Message });
}
```

## Development Workflow

### Building & Running
- **Development**: `dotnet run --project src/VetUberApp.API` (HTTP: 5233, HTTPS: 7155)
- **Testing**: `dotnet test` from root directory
- **MongoDB Setup**: Run `./setup-mongodb.ps1` for local development

### Project Dependencies
- API → Application
- Application → Domain  
- Infrastructure → Domain + Application
- Add new package references to appropriate layer only

### Testing Strategy
- **Unit Tests**: Mock repositories and services in `VetUberApp.UnitTests`
- **Integration Tests**: Use `TestDatabaseFixture` for MongoDB testing
- Test naming: `EntityServiceTests`, `EntityIntegrationTests`

## Key Integration Points

### Health Checks
Configured at `/health/ready` with MongoDB health monitoring. Returns JSON status for all dependencies.

### Swagger Configuration
Auto-generates from XML comments. Access at `/swagger` in development.

### Configuration
- `appsettings.json`: Production settings
- `appsettings.Development.json`: Development overrides
- MongoDB connection string required: `MongoDb:ConnectionString`

## Common Tasks

### Adding New Entity
1. Create entity in `Domain/Entities` inheriting `BaseEntity`
2. Add repository interface in `Domain/Interfaces`
3. Implement repository in `Infrastructure/Persistence/Repositories`
4. Create DTOs in `Application/DTOs`
5. Add service interface and implementation
6. Register dependencies in both `Application` and `Infrastructure` DI
7. Add controller with validation

### Adding Validation
Create validator in `Application/Validators` following pattern:
```csharp
public class CreateEntityDtoValidator : AbstractValidator<CreateEntityDto>
{
    public CreateEntityDtoValidator()
    {
        RuleFor(x => x.Property).NotEmpty().WithMessage("Message");
    }
}
```

Register in `Program.cs`: `builder.Services.AddValidatorsFromAssemblyContaining<ValidatorClass>();`