# VetUberApp - Local Development Setup Script (PowerShell)
# This script sets up the local development environment on Windows

param(
    [switch]$SkipTests,
    [switch]$SkipDocker,
    [string]$MongoPassword = "password"
)

# Colors for output
$RED = "Red"
$GREEN = "Green"
$YELLOW = "Yellow"
$BLUE = "Cyan"

function Write-Status($Message) {
    Write-Host "[INFO] $Message" -ForegroundColor $GREEN
}

function Write-Warning($Message) {
    Write-Host "[WARNING] $Message" -ForegroundColor $YELLOW
}

function Write-Error($Message) {
    Write-Host "[ERROR] $Message" -ForegroundColor $RED
}

function Write-Header($Message) {
    Write-Host "[SETUP] $Message" -ForegroundColor $BLUE
}

# Check if .NET is installed
function Test-DotNet {
    Write-Header "Checking .NET installation..."
    
    try {
        $dotnetVersion = dotnet --version
        Write-Status ".NET SDK version: $dotnetVersion"
        return $true
    }
    catch {
        Write-Error ".NET SDK not found. Please install .NET 8.0 SDK"
        Write-Host "Download from: https://dotnet.microsoft.com/download/dotnet/8.0"
        return $false
    }
}

# Check if Docker is installed and running
function Test-Docker {
    if ($SkipDocker) {
        Write-Warning "Skipping Docker setup as requested"
        return $true
    }
    
    Write-Header "Checking Docker installation..."
    
    try {
        $dockerVersion = docker --version
        Write-Status "Docker version: $dockerVersion"
        
        # Test if Docker daemon is running
        docker info | Out-Null
        Write-Status "Docker is running"
        return $true
    }
    catch {
        Write-Error "Docker is not installed or not running"
        Write-Host "Please install Docker Desktop: https://www.docker.com/products/docker-desktop"
        return $false
    }
}

# Setup MongoDB with Docker
function Start-MongoDB {
    if ($SkipDocker) {
        Write-Warning "Skipping MongoDB setup - make sure you have MongoDB running locally"
        return $true
    }
    
    Write-Header "Setting up MongoDB..."
    
    $containerName = "vetuberapp-mongodb"
    
    # Check if container already exists
    $existingContainer = docker ps -a --filter "name=$containerName" --format "{{.Names}}"
    
    if ($existingContainer -eq $containerName) {
        Write-Warning "MongoDB container already exists"
        
        $runningContainer = docker ps --filter "name=$containerName" --format "{{.Names}}"
        if ($runningContainer -eq $containerName) {
            Write-Status "MongoDB container is already running"
        }
        else {
            Write-Status "Starting existing MongoDB container..."
            docker start $containerName
        }
    }
    else {
        Write-Status "Creating new MongoDB container..."
        docker run -d `
            --name $containerName `
            -p 27017:27017 `
            -e MONGO_INITDB_ROOT_USERNAME=root `
            -e MONGO_INITDB_ROOT_PASSWORD=$MongoPassword `
            -v vetuberapp-mongodb-data:/data/db `
            mongo:7.0
    }
    
    # Wait for MongoDB to be ready
    Write-Status "Waiting for MongoDB to be ready..."
    Start-Sleep -Seconds 15
    
    # Test connection
    try {
        docker exec $containerName mongosh --eval "db.adminCommand('ismaster')" | Out-Null
        Write-Status "MongoDB is ready!"
        return $true
    }
    catch {
        Write-Error "Failed to connect to MongoDB"
        return $false
    }
}

# Restore NuGet packages
function Restore-Packages {
    Write-Header "Restoring NuGet packages..."
    
    try {
        dotnet restore
        Write-Status "Packages restored successfully"
        return $true
    }
    catch {
        Write-Error "Failed to restore packages"
        return $false
    }
}

# Build the solution
function Build-Solution {
    Write-Header "Building solution..."
    
    try {
        dotnet build --configuration Debug --verbosity minimal
        Write-Status "Solution built successfully"
        return $true
    }
    catch {
        Write-Error "Failed to build solution"
        return $false
    }
}

# Run tests
function Invoke-Tests {
    if ($SkipTests) {
        Write-Warning "Skipping tests as requested"
        return $true
    }
    
    Write-Header "Running tests..."
    
    # Set MongoDB connection string for tests
    $env:MongoDb__ConnectionString = "mongodb://root:$MongoPassword@localhost:27017/VetUberAppTest?authSource=admin"
    
    try {
        Write-Status "Running unit tests..."
        dotnet test tests/VetUberApp.UnitTests/VetUberApp.UnitTests.csproj --verbosity minimal --logger "console;verbosity=minimal"
        
        Write-Status "Running integration tests..."
        dotnet test tests/VetUberApp.IntegrationTests/VetUberApp.IntegrationTests.csproj --verbosity minimal --logger "console;verbosity=minimal"
        
        Write-Status "All tests passed!"
        return $true
    }
    catch {
        Write-Error "Some tests failed"
        return $false
    }
}

# Setup development environment files
function Initialize-EnvironmentFiles {
    Write-Header "Setting up environment files..."
    
    $devSettings = "src/VetUberApp.API/appsettings.Development.json"
    
    if (-not (Test-Path $devSettings)) {
        $settingsContent = @"
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "MongoDb": {
    "ConnectionString": "mongodb://root:$MongoPassword@localhost:27017",
    "DatabaseName": "VetUberApp_Development"
  },
  "AllowedHosts": "*"
}
"@
        $settingsContent | Out-File -FilePath $devSettings -Encoding UTF8
        Write-Status "Created development appsettings"
    }
    else {
        Write-Status "Development appsettings already exists"
    }
    
    # Create .env file from example if it doesn't exist
    if (-not (Test-Path ".env") -and (Test-Path ".env.example")) {
        Copy-Item ".env.example" ".env"
        Write-Status "Created .env file from example"
    }
}

# Install development tools
function Install-DevelopmentTools {
    Write-Header "Installing development tools..."
    
    $tools = @(
        @{ Name = "dotnet-ef"; DisplayName = "Entity Framework CLI" },
        @{ Name = "dotnet-outdated-tool"; DisplayName = "dotnet-outdated" },
        @{ Name = "dotnet-reportgenerator-globaltool"; DisplayName = "ReportGenerator" }
    )
    
    foreach ($tool in $tools) {
        try {
            dotnet tool install --global $tool.Name 2>$null
            Write-Status "$($tool.DisplayName) installed"
        }
        catch {
            Write-Status "$($tool.DisplayName) already installed"
        }
    }
}

# Print next steps
function Show-NextSteps {
    Write-Header "Setup completed successfully! 🎉"
    Write-Host ""
    Write-Host "Next steps:" -ForegroundColor $GREEN
    Write-Host "1. Start the API:"
    Write-Host "   cd src\VetUberApp.API"
    Write-Host "   dotnet run"
    Write-Host ""
    Write-Host "2. Open your browser and navigate to:"
    Write-Host "   • API: http://localhost:5233"
    Write-Host "   • Swagger: http://localhost:5233/swagger"
    Write-Host "   • Health Check: http://localhost:5233/health/ready"
    Write-Host ""
    
    if (-not $SkipDocker) {
        Write-Host "3. MongoDB is running on:"
        Write-Host "   • Host: localhost:27017"
        Write-Host "   • Username: root"
        Write-Host "   • Password: $MongoPassword"
        Write-Host ""
    }
    
    Write-Host "4. Useful commands:"
    Write-Host "   • Run tests: dotnet test"
    Write-Host "   • Check outdated packages: dotnet outdated"
    if (-not $SkipDocker) {
        Write-Host "   • Stop MongoDB: docker stop vetuberapp-mongodb"
    }
    Write-Host ""
    Write-Host "Happy coding! 🚀" -ForegroundColor $BLUE
}

# Main execution
function Main {
    Write-Host "🚀 Setting up VetUberApp local development environment..." -ForegroundColor $BLUE
    Write-Host ""
    
    $success = $true
    
    if (-not (Test-DotNet)) { $success = $false }
    if (-not (Test-Docker)) { $success = $false }
    if (-not (Start-MongoDB)) { $success = $false }
    
    if ($success) {
        Initialize-EnvironmentFiles
        Install-DevelopmentTools
        
        if (-not (Restore-Packages)) { $success = $false }
        if (-not (Build-Solution)) { $success = $false }
        if (-not (Invoke-Tests)) { $success = $false }
    }
    
    if ($success) {
        Show-NextSteps
    }
    else {
        Write-Error "Setup completed with errors. Please check the output above."
        exit 1
    }
}

# Help function
function Show-Help {
    Write-Host "VetUberApp Development Setup Script"
    Write-Host ""
    Write-Host "Usage: .\setup-dev.ps1 [OPTIONS]"
    Write-Host ""
    Write-Host "Options:"
    Write-Host "  -SkipTests        Skip running tests"
    Write-Host "  -SkipDocker       Skip Docker/MongoDB setup"
    Write-Host "  -MongoPassword    Set MongoDB password (default: 'password')"
    Write-Host "  -Help             Show this help message"
    Write-Host ""
    Write-Host "Examples:"
    Write-Host "  .\setup-dev.ps1                          # Full setup"
    Write-Host "  .\setup-dev.ps1 -SkipTests               # Skip tests"
    Write-Host "  .\setup-dev.ps1 -SkipDocker              # Skip Docker setup"
    Write-Host "  .\setup-dev.ps1 -MongoPassword 'mypass'  # Custom MongoDB password"
}

# Check for help parameter
if ($args -contains "-Help" -or $args -contains "--help" -or $args -contains "-h") {
    Show-Help
    exit 0
}

# Run main function
Main