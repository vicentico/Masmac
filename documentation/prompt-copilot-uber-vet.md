# Prompt para GitHub Copilot - Aplicación Veterinaria Tipo Uber

## Descripción del Negocio

Crear una **aplicación B2C tipo Uber personalizada para servicios veterinarios** que conecte usuarios con veterinarios cercanos para solicitar atención médica veterinaria a domicilio o en clínicas móviles. La aplicación debe permitir:

- **Geolocalización en tiempo real** para encontrar veterinarios cercanos
- **Reserva de citas inmediatas o programadas** para atención veterinaria
- **Sistema de valoraciones y reseñas** para veterinarios
- **Pagos integrados** con múltiples métodos de pago
- **Seguimiento en tiempo real** del veterinario en camino
- **Chat integrado** entre usuario y veterinario
- **Historial médico** de las mascotas
- **Notificaciones push** para recordatorios y actualizaciones

## Especificaciones Técnicas Requeridas

### Frontend
- **Framework**: Angular 18+ con Ionic 8+
- **Lenguaje**: TypeScript
- **UI Components**: Ionic Components
- **Estado**: NgRx para gestión de estado
- **Mapas**: Google Maps API o Mapbox
- **Notificaciones**: Capacitor Push Notifications
- **Cámara**: Capacitor Camera para fotos de mascotas

### Backend
- **Framework**: ASP.NET Core 8.0+ Web API
- **Lenguaje**: C# 12+
- **Arquitectura**: Clean Architecture (Arquitectura Limpia)
- **Patrones**: Repository Pattern, CQRS, MediatR
- **Principios**: SOLID principles
- **Autenticación**: JWT Bearer Tokens
- **Real-time**: SignalR para tracking y chat
- **Validación**: FluentValidation

### Base de Datos
- **Principal**: MongoDB 7.0+
- **Driver**: MongoDB.Driver para .NET
- **ODM**: Considerara usar MongoFramework o MongoDB.Entities
- **Estructura**: Colecciones para Users, Veterinarians, Appointments, Reviews, Payments

### Infraestructura y Despliegue
- **Contenedorización**: Docker y Docker Compose
- **Cloud**: Azure Container Instances o AWS ECS
- **CI/CD**: GitHub Actions
- **Monitoring**: Application Insights o Prometheus
- **Load Balancer**: NGINX

## Arquitectura del Sistema

### 1. Estructura del Proyecto Backend (Clean Architecture)

```
VetUberApp/
├── src/
│   ├── VetUberApp.Domain/              # Entidades y Reglas de Negocio
│   │   ├── Entities/
│   │   │   ├── User.cs
│   │   │   ├── Veterinarian.cs
│   │   │   ├── Pet.cs
│   │   │   ├── Appointment.cs
│   │   │   ├── Review.cs
│   │   │   └── Payment.cs
│   │   ├── Enums/
│   │   ├── ValueObjects/
│   │   ├── Interfaces/
│   │   └── Common/
│   │       └── BaseEntity.cs
│   │
│   ├── VetUberApp.Application/          # Casos de Uso y Lógica de Aplicación
│   │   ├── Commands/
│   │   ├── Queries/
│   │   ├── Handlers/
│   │   ├── DTOs/
│   │   ├── Interfaces/
│   │   ├── Services/
│   │   ├── Validators/
│   │   └── Mappings/
│   │
│   ├── VetUberApp.Infrastructure/       # Acceso a Datos y Servicios Externos
│   │   ├── Repositories/
│   │   ├── Services/
│   │   ├── Configuration/
│   │   ├── Extensions/
│   │   └── Persistence/
│   │       ├── MongoContext.cs
│   │       └── Configurations/
│   │
│   ├── VetUberApp.API/                  # Controladores y Configuración Web
│   │   ├── Controllers/
│   │   ├── Middleware/
│   │   ├── Hubs/                        # SignalR Hubs
│   │   ├── Extensions/
│   │   ├── Program.cs
│   │   └── appsettings.json
│   │
│   └── VetUberApp.Shared/              # Modelos Compartidos
│       ├── Constants/
│       ├── DTOs/
│       └── Enums/
│
├── tests/                               # Pruebas Unitarias e Integración
├── docker-compose.yml
├── Dockerfile
└── README.md
```

### 2. Estructura del Proyecto Frontend (Angular + Ionic)

```
vet-uber-mobile/
├── src/
│   ├── app/
│   │   ├── core/                        # Servicios Core y Guards
│   │   │   ├── services/
│   │   │   ├── guards/
│   │   │   ├── interceptors/
│   │   │   └── models/
│   │   │
│   │   ├── shared/                      # Componentes Compartidos
│   │   │   ├── components/
│   │   │   ├── directives/
│   │   │   ├── pipes/
│   │   │   └── utils/
│   │   │
│   │   ├── features/                    # Módulos de Características
│   │   │   ├── auth/
│   │   │   ├── home/
│   │   │   ├── booking/
│   │   │   ├── tracking/
│   │   │   ├── profile/
│   │   │   ├── pets/
│   │   │   ├── history/
│   │   │   └── payments/
│   │   │
│   │   ├── store/                       # NgRx Store
│   │   │   ├── actions/
│   │   │   ├── reducers/
│   │   │   ├── effects/
│   │   │   └── selectors/
│   │   │
│   │   └── environments/
│   │
├── capacitor.config.ts
├── ionic.config.json
├── angular.json
├── package.json
└── README.md
```

## Principios SOLID Aplicados

### 1. Single Responsibility Principle (SRP)
- Cada clase debe tener una sola responsabilidad
- Separar entidades, servicios, repositorios y controladores
- Ejemplo: `UserService` solo maneja operaciones de usuario

### 2. Open/Closed Principle (OCP)
- Clases abiertas para extensión, cerradas para modificación
- Usar interfaces y herencia
- Ejemplo: `IPaymentService` con implementaciones específicas

### 3. Liskov Substitution Principle (LSP)
- Objetos derivados deben ser sustituibles por sus clases base
- Implementaciones de interfaces deben ser intercambiables

### 4. Interface Segregation Principle (ISP)
- Interfaces específicas mejor que una general
- Ejemplo: `IUserRepository`, `IVeterinarianRepository` separadas

### 5. Dependency Inversion Principle (DIP)
- Depender de abstracciones, no de implementaciones concretas
- Usar inyección de dependencias en ASP.NET Core

## Buenas Prácticas de Desarrollo

### Backend (.NET)
1. **Async/Await**: Usar programación asíncrona para I/O
2. **Configuration**: Usar IOptions pattern para configuraciones
3. **Logging**: Implementar logging estructurado con Serilog
4. **Exception Handling**: Middleware global de manejo de errores
5. **API Versioning**: Versionado de APIs
6. **Swagger/OpenAPI**: Documentación automática de APIs
7. **Health Checks**: Endpoints de salud para monitoreo
8. **Rate Limiting**: Limitación de requests por usuario

### Frontend (Angular + Ionic)
1. **Lazy Loading**: Carga diferida de módulos
2. **OnPush Strategy**: Estrategia de detección de cambios optimizada
3. **RxJS**: Uso correcto de observables y operators
4. **Error Handling**: Manejo centralizado de errores
5. **TypeScript Strict Mode**: Configuración estricta
6. **Component Communication**: Uso de @Input, @Output y servicios
7. **Performance**: Optimización con TrackBy functions
8. **PWA**: Configuración como Progressive Web App

### MongoDB
1. **Indexing**: Índices apropiados para consultas frecuentes
2. **Schema Design**: Diseño eficiente de documentos
3. **Connection Pooling**: Pool de conexiones
4. **Aggregation Pipelines**: Uso de pipelines para consultas complejas
5. **GridFS**: Para archivos grandes (fotos de mascotas)

## Configuración Docker

### Dockerfile Backend
```dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["src/VetUberApp.API/VetUberApp.API.csproj", "src/VetUberApp.API/"]
COPY ["src/VetUberApp.Application/VetUberApp.Application.csproj", "src/VetUberApp.Application/"]
COPY ["src/VetUberApp.Domain/VetUberApp.Domain.csproj", "src/VetUberApp.Domain/"]
COPY ["src/VetUberApp.Infrastructure/VetUberApp.Infrastructure.csproj", "src/VetUberApp.Infrastructure/"]
RUN dotnet restore "src/VetUberApp.API/VetUberApp.API.csproj"
COPY . .
WORKDIR "/src/src/VetUberApp.API"
RUN dotnet build "VetUberApp.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "VetUberApp.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "VetUberApp.API.dll"]
```

### Docker Compose
```yaml
version: '3.8'

services:
  api:
    build:
      context: .
      dockerfile: Dockerfile
    ports:
      - "5000:80"
      - "5001:443"
    environment:
      - ASPNETCORE_ENVIRONMENT=Development
      - MongoDB__ConnectionString=mongodb://mongo:27017
      - MongoDB__DatabaseName=VetUberDB
    depends_on:
      - mongo
      
  mongo:
    image: mongo:7.0
    ports:
      - "27017:27017"
    volumes:
      - mongodb_data:/data/db
      
  nginx:
    image: nginx:alpine
    ports:
      - "80:80"
    volumes:
      - ./nginx.conf:/etc/nginx/nginx.conf
    depends_on:
      - api

volumes:
  mongodb_data:
```

## Configuración Git Workflow

### Estructura de Ramas
- **main**: Rama principal de producción
- **develop**: Rama de desarrollo
- **feature/**: Ramas para nuevas características
- **hotfix/**: Ramas para correcciones críticas
- **release/**: Ramas para preparar releases

### Commits Convencionales
```
feat: add veterinarian search functionality
fix: resolve payment processing error
docs: update API documentation
style: format code according to guidelines
refactor: improve MongoDB connection handling
test: add unit tests for booking service
chore: update dependencies
```

### GitHub Actions Workflow
```yaml
name: CI/CD Pipeline

on:
  push:
    branches: [ main, develop ]
  pull_request:
    branches: [ main ]

jobs:
  test:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v3
      - name: Setup .NET
        uses: actions/setup-dotnet@v3
        with:
          dotnet-version: '8.0.x'
      - name: Restore dependencies
        run: dotnet restore
      - name: Build
        run: dotnet build --no-restore
      - name: Test
        run: dotnet test --no-build --verbosity normal
        
  deploy:
    needs: test
    runs-on: ubuntu-latest
    if: github.ref == 'refs/heads/main'
    steps:
      - name: Deploy to production
        run: echo "Deploy to production"
```

## Funcionalidades Principales a Implementar

### 1. Autenticación y Autorización
- Registro de usuarios y veterinarios
- Login con JWT
- Verificación de identidad de veterinarios
- Roles y permisos

### 2. Geolocalización y Búsqueda
- Localizar veterinarios cercanos
- Filtros por especialidad, calificación, disponibilidad
- Cálculo de distancias y tiempo estimado

### 3. Sistema de Reservas
- Reserva inmediata o programada
- Selección de servicios veterinarios
- Confirmación de citas

### 4. Tracking en Tiempo Real
- Seguimiento de veterinario en camino
- Updates de estado de la cita
- ETA (tiempo estimado de llegada)

### 5. Pagos
- Integración con pasarelas de pago
- Múltiples métodos de pago
- Facturación automática

### 6. Valoraciones y Reseñas
- Sistema de calificación
- Comentarios de usuarios
- Historial de reseñas

### 7. Gestión de Mascotas
- Perfiles de mascotas
- Historial médico
- Documentos veterinarios

### 8. Notificaciones
- Push notifications
- Recordatorios de citas
- Actualizaciones de estado

## Consideraciones de Escalabilidad

1. **Microservicios**: Arquitectura preparada para dividir en microservicios
2. **Caching**: Implementar Redis para caché
3. **Load Balancing**: NGINX o Azure Load Balancer
4. **Database Sharding**: Preparar MongoDB para sharding
5. **CDN**: Para contenido estático y imágenes
6. **Monitoring**: Application Performance Monitoring

## Seguridad

1. **HTTPS**: Comunicación segura
2. **Input Validation**: Validación de entrada
3. **SQL Injection Prevention**: Usar parámetros en consultas
4. **XSS Protection**: Sanitización de datos
5. **CORS**: Configuración correcta de CORS
6. **Rate Limiting**: Prevención de ataques DDoS
7. **Data Encryption**: Cifrado de datos sensibles

## Testing Strategy

1. **Unit Tests**: xUnit para backend, Jasmine/Karma para frontend
2. **Integration Tests**: Testing de APIs
3. **E2E Tests**: Cypress para pruebas de extremo a extremo
4. **Performance Tests**: Load testing con herramientas como k6

## Instrucciones de Uso

1. **Generar proyecto backend**: Crear solución .NET con estructura Clean Architecture
2. **Configurar MongoDB**: Setup de conexión y modelos
3. **Implementar CRUD**: Operaciones básicas para todas las entidades
4. **Agregar autenticación**: JWT y roles
5. **Crear APIs REST**: Controladores con Swagger
6. **Implementar SignalR**: Para funcionalidades en tiempo real
7. **Setup Docker**: Containerización y compose
8. **Configurar CI/CD**: GitHub Actions pipeline
9. **Crear frontend Ionic**: Estructura Angular con Ionic
10. **Integrar servicios**: Conectar frontend con backend APIs

Por favor, genera el código base siguiendo estas especificaciones, aplicando las mejores prácticas mencionadas y asegurando que el código sea escalable, mantenible y siga los principios SOLID.