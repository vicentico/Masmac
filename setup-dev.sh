#!/bin/bash

# VetUberApp - Local Development Setup Script
# This script sets up the local development environment

set -e

echo "🚀 Setting up VetUberApp local development environment..."

# Colors for output
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
BLUE='\033[0;34m'
NC='\033[0m' # No Color

# Function to print colored output
print_status() {
    echo -e "${GREEN}[INFO]${NC} $1"
}

print_warning() {
    echo -e "${YELLOW}[WARNING]${NC} $1"
}

print_error() {
    echo -e "${RED}[ERROR]${NC} $1"
}

print_header() {
    echo -e "${BLUE}[SETUP]${NC} $1"
}

# Check if .NET is installed
check_dotnet() {
    print_header "Checking .NET installation..."
    if command -v dotnet &> /dev/null; then
        DOTNET_VERSION=$(dotnet --version)
        print_status ".NET SDK version: $DOTNET_VERSION"
    else
        print_error ".NET SDK not found. Please install .NET 8.0 SDK"
        exit 1
    fi
}

# Check if Docker is installed and running
check_docker() {
    print_header "Checking Docker installation..."
    if command -v docker &> /dev/null; then
        if docker info &> /dev/null; then
            print_status "Docker is installed and running"
        else
            print_error "Docker is installed but not running. Please start Docker"
            exit 1
        fi
    else
        print_error "Docker not found. Please install Docker"
        exit 1
    fi
}

# Setup MongoDB with Docker
setup_mongodb() {
    print_header "Setting up MongoDB..."
    
    CONTAINER_NAME="vetuberapp-mongodb"
    
    # Check if container already exists
    if docker ps -a | grep -q $CONTAINER_NAME; then
        print_warning "MongoDB container already exists"
        if docker ps | grep -q $CONTAINER_NAME; then
            print_status "MongoDB container is already running"
        else
            print_status "Starting existing MongoDB container..."
            docker start $CONTAINER_NAME
        fi
    else
        print_status "Creating new MongoDB container..."
        docker run -d \
            --name $CONTAINER_NAME \
            -p 27017:27017 \
            -e MONGO_INITDB_ROOT_USERNAME=root \
            -e MONGO_INITDB_ROOT_PASSWORD=password \
            -v vetuberapp-mongodb-data:/data/db \
            mongo:7.0
    fi
    
    # Wait for MongoDB to be ready
    print_status "Waiting for MongoDB to be ready..."
    sleep 10
    
    # Test connection
    if docker exec $CONTAINER_NAME mongosh --eval "db.adminCommand('ismaster')" &> /dev/null; then
        print_status "MongoDB is ready!"
    else
        print_error "Failed to connect to MongoDB"
        exit 1
    fi
}

# Restore NuGet packages
restore_packages() {
    print_header "Restoring NuGet packages..."
    dotnet restore
    print_status "Packages restored successfully"
}

# Build the solution
build_solution() {
    print_header "Building solution..."
    dotnet build --configuration Debug
    print_status "Solution built successfully"
}

# Run tests
run_tests() {
    print_header "Running tests..."
    
    # Set MongoDB connection string for tests
    export MongoDb__ConnectionString="mongodb://root:password@localhost:27017/VetUberAppTest?authSource=admin"
    
    print_status "Running unit tests..."
    dotnet test tests/VetUberApp.UnitTests/VetUberApp.UnitTests.csproj --verbosity minimal
    
    print_status "Running integration tests..."
    dotnet test tests/VetUberApp.IntegrationTests/VetUberApp.IntegrationTests.csproj --verbosity minimal
    
    print_status "All tests passed!"
}

# Setup development environment files
setup_env_files() {
    print_header "Setting up environment files..."
    
    # Create development appsettings if it doesn't exist
    DEV_SETTINGS="src/VetUberApp.API/appsettings.Development.json"
    if [ ! -f "$DEV_SETTINGS" ]; then
        cat > "$DEV_SETTINGS" << 'EOF'
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "MongoDb": {
    "ConnectionString": "mongodb://root:password@localhost:27017",
    "DatabaseName": "VetUberApp_Development"
  },
  "AllowedHosts": "*"
}
EOF
        print_status "Created development appsettings"
    else
        print_status "Development appsettings already exists"
    fi
}

# Install development tools
install_dev_tools() {
    print_header "Installing development tools..."
    
    # Install useful dotnet tools
    dotnet tool install --global dotnet-ef 2>/dev/null && print_status "Entity Framework CLI installed" || print_status "Entity Framework CLI already installed"
    dotnet tool install --global dotnet-outdated-tool 2>/dev/null && print_status "dotnet-outdated installed" || print_status "dotnet-outdated already installed"
    dotnet tool install --global dotnet-reportgenerator-globaltool 2>/dev/null && print_status "ReportGenerator installed" || print_status "ReportGenerator already installed"
}

# Print next steps
print_next_steps() {
    print_header "Setup completed successfully! 🎉"
    echo ""
    echo -e "${GREEN}Next steps:${NC}"
    echo "1. Start the API:"
    echo "   cd src/VetUberApp.API && dotnet run"
    echo ""
    echo "2. Open your browser and navigate to:"
    echo "   • API: http://localhost:5233"
    echo "   • Swagger: http://localhost:5233/swagger"
    echo "   • Health Check: http://localhost:5233/health/ready"
    echo ""
    echo "3. MongoDB is running on:"
    echo "   • Host: localhost:27017"
    echo "   • Username: root"
    echo "   • Password: password"
    echo ""
    echo "4. Useful commands:"
    echo "   • Run tests: dotnet test"
    echo "   • Check outdated packages: dotnet outdated"
    echo "   • Stop MongoDB: docker stop vetuberapp-mongodb"
    echo ""
    echo -e "${BLUE}Happy coding! 🚀${NC}"
}

# Main execution
main() {
    check_dotnet
    check_docker
    setup_mongodb
    setup_env_files
    install_dev_tools
    restore_packages
    build_solution
    run_tests
    print_next_steps
}

# Run main function
main