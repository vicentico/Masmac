# Navegar al directorio raíz del proyecto
Set-Location -Path "D:\Cursor_AI\APP_masmac\Masmac\vet-uber-app"

# Agregar referencia de Domain a Infrastructure
Set-Location -Path "src\VetUberApp.Infrastructure"
dotnet add reference "..\VetUberApp.Domain\VetUberApp.Domain.csproj"

# Instalar paquetes NuGet en Infrastructure
dotnet add package MongoDB.Driver
dotnet add package Microsoft.Extensions.Options
dotnet add package Microsoft.Extensions.Configuration
dotnet add package Microsoft.Extensions.Configuration.Binder
dotnet add package Microsoft.Extensions.DependencyInjection

# Volver al directorio raíz y compilar
Set-Location -Path "..\.."
dotnet build