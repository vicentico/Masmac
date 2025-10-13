# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution file
COPY VetUberApp.sln .

# Copy project files
COPY src/VetUberApp.API/VetUberApp.API.csproj src/VetUberApp.API/
COPY src/VetUberApp.Application/VetUberApp.Application.csproj src/VetUberApp.Application/
COPY src/VetUberApp.Domain/VetUberApp.Domain.csproj src/VetUberApp.Domain/
COPY src/VetUberApp.Infrastructure/VetUberApp.Infrastructure.csproj src/VetUberApp.Infrastructure/
COPY src/VetUberApp.Shared/VetUberApp.Shared.csproj src/VetUberApp.Shared/

# Restore dependencies only for the main projects
RUN dotnet restore src/VetUberApp.API/VetUberApp.API.csproj

# Copy source code
COPY src/ src/

# Build the application
RUN dotnet build src/VetUberApp.API/VetUberApp.API.csproj -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish src/VetUberApp.API/VetUberApp.API.csproj -c Release -o /app/publish --no-restore

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Create non-root user
RUN groupadd -r appuser && useradd -r -g appuser appuser

# Copy published application
COPY --from=publish /app/publish .

# Set ownership
RUN chown -R appuser:appuser /app
USER appuser

# Expose port
EXPOSE 8080

# Configure ASP.NET Core to bind to port 8080
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

# Health check
HEALTHCHECK --interval=30s --timeout=10s --start-period=30s --retries=3 \
  CMD curl -f http://localhost:8080/health/ready || exit 1

# Start the application
ENTRYPOINT ["dotnet", "VetUberApp.API.dll"]