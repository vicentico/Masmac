using Microsoft.Extensions.DependencyInjection;
using VetUberApp.Application.DTOs;
using VetUberApp.Application.Interfaces;
using VetUberApp.Domain.Entities;
using VetUberApp.Domain.Enums;
using VetUberApp.Domain.ValueObjects;
using VetUberApp.Domain.Constants;
using FluentAssertions;
using Xunit;

namespace VetUberApp.IntegrationTests;

[Collection("IntegrationTests")]
public class UserIntegrationTests : IClassFixture<TestDatabaseFixture>
{
    private readonly IUserService _userService;
    private readonly TestDatabaseFixture _fixture;

    public UserIntegrationTests(TestDatabaseFixture fixture)
    {
        _fixture = fixture;
        _userService = _fixture.ServiceProvider.GetRequiredService<IUserService>();
    }

    [Fact]
    public async Task CreateUser_WithValidData_ShouldSucceed()
    {
        // Arrange
        var createUserDto = new CreateUserDto(
            Email: $"test{Guid.NewGuid()}@example.com",
            Password: "password123",
            FirstName: "Juan",
            LastName: "Pérez",
            PhoneNumber: "+34123456789",
            Address: new Address
            {
                Street = "Calle Mayor 123",
                City = "Madrid",
                State = "Madrid",
                Country = "España",
                ZipCode = "28001",
                Latitude = 40.4168,
                Longitude = -3.7038
            }
        );

        // Act
        var createdUser = await _userService.CreateAsync(createUserDto);

        // Assert
        createdUser.Should().NotBeNull();
        createdUser.Id.Should().NotBeNullOrEmpty();
        createdUser.Email.Should().Be(createUserDto.Email);
        createdUser.FirstName.Should().Be(createUserDto.FirstName);
        createdUser.LastName.Should().Be(createUserDto.LastName);
        createdUser.PhoneNumber.Should().Be(createUserDto.PhoneNumber);
        createdUser.Role.Should().Be(UserRole.Client);
        createdUser.IsVerified.Should().BeFalse();
        createdUser.Address.Should().NotBeNull();
        createdUser.Address.Street.Should().Be(createUserDto.Address.Street);
        createdUser.Address.City.Should().Be(createUserDto.Address.City);
    }

    [Fact]
    public async Task CreateUser_WithDuplicateEmail_ShouldThrowException()
    {
        // Arrange
        var email = $"duplicate{Guid.NewGuid()}@example.com";
        var address = new Address
        {
            Street = "Calle Mayor 123",
            City = "Madrid",
            State = "Madrid",
            Country = "España",
            ZipCode = "28001",
            Latitude = 40.4168,
            Longitude = -3.7038
        };

        var firstUserDto = new CreateUserDto(
            Email: email,
            Password: "password123",
            FirstName: "Juan",
            LastName: "Pérez",
            PhoneNumber: "+34123456789",
            Address: address
        );

        var secondUserDto = new CreateUserDto(
            Email: email, // Same email
            Password: "password456",
            FirstName: "María",
            LastName: "García",
            PhoneNumber: "+34987654321",
            Address: address
        );

        // Act & Assert
        await _userService.CreateAsync(firstUserDto);

        var exception = await Assert.ThrowsAsync<InvalidOperationException>(
            () => _userService.CreateAsync(secondUserDto));

        exception.Message.Should().Be(ErrorConstants.Users.EmailAlreadyExists);
    }

    [Fact]
    public async Task GetUserById_ExistingUser_ShouldReturnUser()
    {
        // Arrange
        var createUserDto = new CreateUserDto(
            Email: $"get{Guid.NewGuid()}@example.com",
            Password: "password123",
            FirstName: "Ana",
            LastName: "López",
            PhoneNumber: "+34111222333",
            Address: new Address
            {
                Street = "Avenida Principal 456",
                City = "Barcelona",
                State = "Cataluña",
                Country = "España",
                ZipCode = "08001",
                Latitude = 41.3851,
                Longitude = 2.1734
            }
        );

        var createdUser = await _userService.CreateAsync(createUserDto);

        // Act
        var retrievedUser = await _userService.GetByIdAsync(createdUser.Id);

        // Assert
        retrievedUser.Should().NotBeNull();
        retrievedUser!.Id.Should().Be(createdUser.Id);
        retrievedUser.Email.Should().Be(createUserDto.Email);
        retrievedUser.FirstName.Should().Be(createUserDto.FirstName);
        retrievedUser.LastName.Should().Be(createUserDto.LastName);
    }

    [Fact]
    public async Task GetUserById_NonExistentUser_ShouldReturnNull()
    {
        // Arrange
        var nonExistentId = "507f1f77bcf86cd799439011"; // Valid ObjectId format

        // Act
        var result = await _userService.GetByIdAsync(nonExistentId);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task UpdateUser_ExistingUser_ShouldUpdateSuccessfully()
    {
        // Arrange
        var createUserDto = new CreateUserDto(
            Email: $"update{Guid.NewGuid()}@example.com",
            Password: "password123",
            FirstName: "Carlos",
            LastName: "Martín",
            PhoneNumber: "+34444555666",
            Address: new Address
            {
                Street = "Plaza Central 789",
                City = "Valencia",
                State = "Valencia",
                Country = "España",
                ZipCode = "46001",
                Latitude = 39.4699,
                Longitude = -0.3763
            }
        );

        var createdUser = await _userService.CreateAsync(createUserDto);

        var updateUserDto = new UpdateUserDto(
            FirstName: "Carlos Updated",
            LastName: "Martín Updated",
            PhoneNumber: "+34777888999",
            ProfilePictureUrl: "https://example.com/photo.jpg",
            Address: new Address
            {
                Street = "Nueva Calle 321",
                City = "Sevilla",
                State = "Andalucía",
                Country = "España",
                ZipCode = "41001",
                Latitude = 37.3891,
                Longitude = -5.9845
            }
        );

        // Act
        var updatedUser = await _userService.UpdateAsync(createdUser.Id, updateUserDto);

        // Assert
        updatedUser.Should().NotBeNull();
        updatedUser.Id.Should().Be(createdUser.Id);
        updatedUser.FirstName.Should().Be(updateUserDto.FirstName);
        updatedUser.LastName.Should().Be(updateUserDto.LastName);
        updatedUser.PhoneNumber.Should().Be(updateUserDto.PhoneNumber);
        updatedUser.ProfilePictureUrl.Should().Be(updateUserDto.ProfilePictureUrl);
        updatedUser.Address.Street.Should().Be(updateUserDto.Address.Street);
        updatedUser.Address.City.Should().Be(updateUserDto.Address.City);
    }

    [Fact]
    public async Task UpdateUser_NonExistentUser_ShouldThrowException()
    {
        // Arrange
        var nonExistentId = "507f1f77bcf86cd799439012"; // Valid ObjectId format
        var updateUserDto = new UpdateUserDto(
            FirstName: "Test",
            LastName: "User",
            PhoneNumber: "+34123456789",
            ProfilePictureUrl: null,
            Address: new Address
            {
                Street = "Test Street",
                City = "Test City",
                State = "Test State",
                Country = "Test Country",
                ZipCode = "12345",
                Latitude = 0,
                Longitude = 0
            }
        );

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(
            () => _userService.UpdateAsync(nonExistentId, updateUserDto));

        exception.Message.Should().Be(ErrorConstants.Users.NotFound);
    }

    [Fact]
    public async Task DeleteUser_ExistingUser_ShouldDeleteSuccessfully()
    {
        // Arrange
        var createUserDto = new CreateUserDto(
            Email: $"delete{Guid.NewGuid()}@example.com",
            Password: "password123",
            FirstName: "Eliminado",
            LastName: "Usuario",
            PhoneNumber: "+34999888777",
            Address: new Address
            {
                Street = "Calle Temporal 999",
                City = "Temporal",
                State = "Temporal",
                Country = "España",
                ZipCode = "99999",
                Latitude = 0,
                Longitude = 0
            }
        );

        var createdUser = await _userService.CreateAsync(createUserDto);

        // Act
        await _userService.DeleteAsync(createdUser.Id);

        // Assert
        var deletedUser = await _userService.GetByIdAsync(createdUser.Id);
        deletedUser.Should().BeNull();
    }

    [Fact]
    public async Task GetAllUsers_ShouldReturnMultipleUsers()
    {
        // Arrange
        var users = new List<CreateUserDto>();
        for (int i = 0; i < 3; i++)
        {
            users.Add(new CreateUserDto(
                Email: $"getall{i}{Guid.NewGuid()}@example.com",
                Password: "password123",
                FirstName: $"User{i}",
                LastName: $"Test{i}",
                PhoneNumber: $"+3412345678{i}",
                Address: new Address
                {
                    Street = $"Street {i}",
                    City = $"City {i}",
                    State = $"State {i}",
                    Country = "España",
                    ZipCode = $"1234{i}",
                    Latitude = i,
                    Longitude = i
                }
            ));
        }

        // Create users
        foreach (var userDto in users)
        {
            await _userService.CreateAsync(userDto);
        }

        // Act
        var allUsers = await _userService.GetAllAsync();

        // Assert
        allUsers.Should().HaveCountGreaterOrEqualTo(3);
        foreach (var userDto in users)
        {
            allUsers.Should().Contain(u => u.Email == userDto.Email);
        }
    }
}