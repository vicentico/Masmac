using Microsoft.Extensions.DependencyInjection;
using VetUberApp.Application.Interfaces;
using VetUberApp.Infrastructure.Common;
using VetUberApp.Application.DTOs;
using FluentAssertions;

namespace VetUberApp.IntegrationTests;

/// <summary>
/// Pruebas para verificar el manejo correcto de IDs inválidos de MongoDB
/// </summary>
public class ObjectIdValidationTests : IClassFixture<TestDatabaseFixture>
{
    private readonly TestDatabaseFixture _fixture;
    private readonly IUserService _userService;

    public ObjectIdValidationTests(TestDatabaseFixture fixture)
    {
        _fixture = fixture;
        _userService = fixture.ServiceProvider.GetRequiredService<IUserService>();
    }

    [Fact]
    public void MongoDbHelper_IsValidObjectId_WithValidId_ShouldReturnTrue()
    {
        // Arrange
        var validId = "68c613ddcd15a7ebad111654";

        // Act
        var result = MongoDbHelper.IsValidObjectId(validId);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void MongoDbHelper_IsValidObjectId_WithInvalidId_ShouldReturnFalse()
    {
        // Arrange
        var invalidId = "invalid-id-123";

        // Act
        var result = MongoDbHelper.IsValidObjectId(invalidId);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void MongoDbHelper_ValidateObjectId_WithInvalidId_ShouldThrowArgumentException()
    {
        // Arrange
        var invalidId = "invalid-id-123";

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => 
            MongoDbHelper.ValidateObjectId(invalidId));
        
        exception.Message.Should().Contain("no es un ObjectId válido de MongoDB");
    }

    [Fact]
    public async Task UserService_GetByIdAsync_WithInvalidId_ShouldReturnNull()
    {
        // Arrange
        var invalidId = "invalid-id-123";

        // Act
        var result = await _userService.GetByIdAsync(invalidId);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task UserService_GetByIdAsync_WithValidButNonExistentId_ShouldReturnNull()
    {
        // Arrange
        var nonExistentValidId = "68c613ddcd15a7ebad999999";

        // Act
        var result = await _userService.GetByIdAsync(nonExistentValidId);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task UserService_DeleteAsync_WithInvalidId_ShouldReturnFalse()
    {
        // Arrange
        var invalidId = "invalid-id-123";

        // Act
        var result = await _userService.DeleteAsync(invalidId);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public async Task UserService_DeleteAsync_WithValidButNonExistentId_ShouldReturnFalse()
    {
        // Arrange
        var nonExistentValidId = "68c613ddcd15a7ebad999999";

        // Act
        var result = await _userService.DeleteAsync(nonExistentValidId);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public async Task UserService_UpdateAsync_WithInvalidId_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var invalidId = "invalid-id-123";
        var updateDto = new UpdateUserDto(
            FirstName: "Updated",
            LastName: "User",
            PhoneNumber: "+1234567890",
            ProfilePictureUrl: null,
            Address: new AddressDto(
                Street: "Updated Street",
                City: "Updated City",
                State: "Updated State",
                ZipCode: "12345",
                Country: "Updated Country"
            )
        );

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _userService.UpdateAsync(invalidId, updateDto));
        
        exception.Message.Should().Contain("No se encontró el usuario");
    }
}