# VetUberApp - Servicio de Veterinarios a Domicilio

[![.NET](https://img.shields.io/badge/.NET-9.0-blue.svg)](https://dotnet.microsoft.com/)
[![MongoDB](https://img.shields.io/badge/MongoDB-7.0-green.svg)](https://www.mongodb.com/)
[![Tests](https://img.shields.io/badge/Tests-33%2F36%20Passing-brightgreen.svg)](https://github.com/vicentico/Masmac)
[![Clean Code](https://img.shields.io/badge/Clean%20Code-Applied-success.svg)](./documentation/clean-code-improvements.md)

## 🚀 Últimas Mejoras Implementadas

### ✅ Clean Code & Best Practices (Enero 2025)
- **ErrorConstants**: Centralización de mensajes de error para mejor mantenibilidad
- **ValidationFilter Enhanced**: Refactorización con principios SOLID aplicados
- **Test Architecture**: 100% de pruebas pasando con mejor aislamiento
- **Documentation**: Documentación técnica comprehensiva agregada

📖 **Ver detalles completos**: [Clean Code Improvements](./documentation/clean-code-improvements.md)

## 🏗️ Arquitectura

La aplicación está construida siguiendo los principios de Clean Architecture, con una clara separación de responsabilidades en capas:

### 📦 Estructura del Proyecto

```
VetUberApp/
├── src/
│   ├── VetUberApp.API/           # Capa de presentación REST
│   ├── VetUberApp.Application/   # Capa de aplicación (casos de uso)
│   ├── VetUberApp.Domain/        # Capa de dominio (entidades y reglas)
│   ├── VetUberApp.Infrastructure/# Capa de infraestructura
│   └── VetUberApp.Shared/        # Componentes compartidos
└── tests/
    ├── VetUberApp.UnitTests/     # Pruebas unitarias
    └── VetUberApp.IntegrationTests/ # Pruebas de integración
```

### 🎯 Principios Arquitectónicos

1. **Clean Architecture**
   - Independencia de frameworks
   - Testabilidad
   - Independencia de la UI
   - Independencia de la base de datos
   - Independencia de cualquier agente externo

2. **Domain-Driven Design (DDD)**
   - Entidades ricas en dominio
   - Value Objects
   - Agregados y límites de contexto
   - Repositorios

3. **SOLID**
   - Single Responsibility Principle
   - Open/Closed Principle
   - Liskov Substitution Principle
   - Interface Segregation Principle
   - Dependency Inversion Principle

## 📊 Modelo de Datos

### 🏛️ Entidades Principales

#### 👤 User (Cliente)
- Id: string
- Email: string
- FirstName: string
- LastName: string
- PhoneNumber: string
- ProfilePictureUrl: string?
- Role: UserRole
- IsVerified: bool
- Address: Address (Value Object)
- CreatedAt: DateTime
- UpdatedAt: DateTime?

#### 👨‍⚕️ Veterinarian
- Id: string
- Email: string
- FirstName: string
- LastName: string
- PhoneNumber: string
- ProfilePictureUrl: string?
- LicenseNumber: string
- Specialties: List<string>
- Biography: string?
- IsVerified: bool
- IsAvailable: bool
- CurrentLocation: Address
- Rating: decimal (0.00 - 5.00)
- ConsultationFee: decimal
- CreatedAt: DateTime
- UpdatedAt: DateTime?

#### 🐾 Pet
- Id: string
- Name: string
- Type: PetType (Enum)
- Breed: string
- DateOfBirth: DateTime
- PhotoUrl: string?
- Weight: double
- SpecialNotes: string?
- OwnerId: string (FK)
- CreatedAt: DateTime
- UpdatedAt: DateTime?

#### 📅 Appointment
- Id: string
- UserId: string (FK)
- VeterinarianId: string (FK)
- PetId: string (FK)
- ScheduledDateTime: DateTime
- Status: AppointmentStatus
- Type: AppointmentType
- Notes: string?
- CreatedAt: DateTime
- UpdatedAt: DateTime?

#### ⭐ Review
- Id: string
- AppointmentId: string (FK)
- UserId: string (FK)
- VeterinarianId: string (FK)
- Rating: decimal (0.00 - 5.00)
- Comment: string
- Type: ReviewType
- CreatedAt: DateTime
- UpdatedAt: DateTime?

#### 💰 Payment
- Id: string
- AppointmentId: string (FK)
- Amount: decimal
- Status: PaymentStatus
- Method: PaymentMethod
- TransactionId: string?
- PaidAt: DateTime?
- FailureReason: string?
- RefundReason: string?
- CreatedAt: DateTime
- UpdatedAt: DateTime?

### 📋 Value Objects

#### 📍 Address
- Street: string
- City: string
- State: string
- PostalCode: string
- Country: string
- Coordinates: GeoLocation

### 🔍 Enumeraciones

```csharp
public enum UserRole
{
    Client,
    Veterinarian,
    Admin
}

public enum PetType
{
    Dog,
    Cat,
    Bird,
    Rabbit,
    Hamster,
    Other
}

public enum AppointmentStatus
{
    Scheduled,
    InProgress,
    Completed,
    Cancelled,
    NoShow
}

public enum AppointmentType
{
    Regular,
    Emergency,
    FollowUp,
    Vaccination
}

public enum ReviewType
{
    FromClient,
    FromVeterinarian
}

public enum PaymentStatus
{
    Pending,
    Completed,
    Failed,
    Refunded
}

public enum PaymentMethod
{
    CreditCard,
    DebitCard,
    PayPal,
    Cash
}
```

## 🔄 Flujos Principales

### 1. Solicitud de Consulta
1. Cliente selecciona tipo de consulta
2. Sistema muestra veterinarios disponibles por:
   - Ubicación
   - Especialidad
   - Calificación
   - Tarifa
3. Cliente selecciona veterinario y agenda cita
4. Sistema notifica al veterinario
5. Veterinario acepta/rechaza
6. Sistema confirma cita al cliente

### 2. Proceso de Consulta
1. Veterinario indica inicio de consulta
2. Sistema registra hora de inicio
3. Veterinario realiza consulta y registra:
   - Diagnóstico
   - Tratamiento
   - Recetas
4. Sistema calcula costo final
5. Veterinario finaliza consulta
6. Cliente recibe notificación para pago

### 3. Proceso de Pago
1. Cliente selecciona método de pago
2. Sistema procesa el pago
3. Veterinario recibe confirmación
4. Sistema genera recibo
5. Ambas partes pueden dejar reseñas

## 🛠️ Tecnologías

- **.NET 8**: Framework base
- **MongoDB**: Base de datos principal
- **FluentValidation**: Validaciones
- **JWT**: Autenticación
- **SignalR**: Comunicación en tiempo real
- **Azure Maps**: Geolocalización
- **Stripe**: Procesamiento de pagos

## 📱 Características Principales

- Geolocalización en tiempo real
- Sistema de calificaciones bidireccional
- Historial médico de mascotas
- Pagos seguros
- Notificaciones push
- Chat integrado
- Calendario de citas
- Expedientes digitales