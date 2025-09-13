using MongoDB.Driver;
using Moq;
using VetUberApp.Domain.Entities;
using VetUberApp.Domain.Enums;
using VetUberApp.Infrastructure.Persistence;
using VetUberApp.Infrastructure.Persistence.Repositories;
using VetUberApp.UnitTests.Mocks;
using FluentAssertions;

namespace VetUberApp.UnitTests.Repositories;

public class ReviewRepositoryTests
{
    private readonly Mock<IMongoCollection<Review>> _mockCollection;
    private readonly MongoDbContext _mockContext;
    private readonly ReviewRepository _repository;

    public ReviewRepositoryTests()
    {
        // Setup mock collection
        _mockCollection = new Mock<IMongoCollection<Review>>();
        
        // Setup mock context
        var mockContext = new Mock<MongoDbContext>();
        mockContext.Setup(c => c.GetCollection<Review>("Reviews"))
            .Returns(_mockCollection.Object);
        _mockContext = mockContext.Object;

        // Setup default behaviors
        _mockCollection.Setup(c => c.InsertOneAsync(
            It.IsAny<Review>(),
            It.IsAny<InsertOneOptions>(),
            It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var mockCursor = new Mock<IAsyncCursor<Review>>();
        mockCursor.Setup(c => c.Current).Returns(new List<Review>());
        mockCursor.SetupSequence(c => c.MoveNext(It.IsAny<CancellationToken>()))
            .Returns(true)
            .Returns(false);
        mockCursor.SetupSequence(c => c.MoveNextAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(true))
            .Returns(Task.FromResult(false));

        _mockCollection.Setup(c => c.FindAsync(
            It.IsAny<FilterDefinition<Review>>(),
            It.IsAny<FindOptions<Review>>(),
            It.IsAny<CancellationToken>()))
            .ReturnsAsync(mockCursor.Object);

        _repository = new ReviewRepository(_mockContext);
    }

    [Fact]
    public async Task CreateAsync_ShouldCreateReview()
    {
        // Arrange
        var review = new Review
        {
            AppointmentId = "appointment123",
            UserId = "user123",
            VeterinarianId = "vet123",
            Rating = 5,
            Comment = "Excellent service",
            Type = ReviewType.Overall
        };

        _mockCollection.Setup(c => c.InsertOneAsync(
            It.IsAny<Review>(),
            It.IsAny<InsertOneOptions>(),
            It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _repository.CreateAsync(review);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().NotBeNullOrEmpty();
        result.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public async Task GetByVeterinarianIdAsync_ShouldReturnReviews()
    {
        // Arrange
        var veterinarianId = "vet123";
        var reviews = new List<Review>
        {
            new Review { VeterinarianId = veterinarianId, Rating = 5 },
            new Review { VeterinarianId = veterinarianId, Rating = 4 }
        };

        var mockCursor = new Mock<IAsyncCursor<Review>>();
        mockCursor.Setup(c => c.Current).Returns(reviews);
        mockCursor.SetupSequence(c => c.MoveNext(It.IsAny<CancellationToken>()))
            .Returns(true)
            .Returns(false);
        mockCursor.SetupSequence(c => c.MoveNextAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(true))
            .Returns(Task.FromResult(false));

        _mockCollection.Setup(c => c.FindAsync(
            It.IsAny<FilterDefinition<Review>>(),
            It.IsAny<FindOptions<Review>>(),
            It.IsAny<CancellationToken>()))
            .ReturnsAsync(mockCursor.Object);

        // Act
        var result = await _repository.GetReviewsByVeterinarianId(veterinarianId);

        // Assert
        result.Should().HaveCount(2);
        result.Should().AllSatisfy(r => r.VeterinarianId.Should().Be(veterinarianId));
    }

    [Fact]
    public async Task GetAverageRatingByVeterinarianIdAsync_ShouldCalculateAverage()
    {
        // Arrange
        var veterinarianId = "vet123";
        var reviews = new List<Review>
        {
            new Review { VeterinarianId = veterinarianId, Rating = 5 },
            new Review { VeterinarianId = veterinarianId, Rating = 4 },
            new Review { VeterinarianId = veterinarianId, Rating = 3 }
        };

        var mockCursor = new Mock<IAsyncCursor<Review>>();
        mockCursor.Setup(c => c.Current).Returns(reviews);
        mockCursor.SetupSequence(c => c.MoveNext(It.IsAny<CancellationToken>()))
            .Returns(true)
            .Returns(false);
        mockCursor.SetupSequence(c => c.MoveNextAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(true))
            .Returns(Task.FromResult(false));

        _mockCollection.Setup(c => c.FindAsync(
            It.IsAny<FilterDefinition<Review>>(),
            It.IsAny<FindOptions<Review>>(),
            It.IsAny<CancellationToken>()))
            .ReturnsAsync(mockCursor.Object);

        // Act
        var result = await _repository.GetAverageRatingByVeterinarianIdAsync(veterinarianId);

        // Assert
        result.Should().Be(4.0m); // (5 + 4 + 3) / 3 = 4
    }

    [Fact]
    public async Task HasUserReviewedAppointmentAsync_ShouldReturnTrue_WhenReviewExists()
    {
        // Arrange
        var userId = "user123";
        var appointmentId = "appointment123";
        var review = new Review 
        { 
            UserId = userId, 
            AppointmentId = appointmentId 
        };

        var mockCursor = new Mock<IAsyncCursor<Review>>();
        mockCursor.Setup(c => c.Current).Returns(new List<Review> { review });
        mockCursor.SetupSequence(c => c.MoveNext(It.IsAny<CancellationToken>()))
            .Returns(true)
            .Returns(false);
        mockCursor.SetupSequence(c => c.MoveNextAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(true))
            .Returns(Task.FromResult(false));

        _mockCollection.Setup(c => c.FindAsync(
            It.IsAny<FilterDefinition<Review>>(),
            It.IsAny<FindOptions<Review>>(),
            It.IsAny<CancellationToken>()))
            .ReturnsAsync(mockCursor.Object);

        // Act
        var result = await _repository.HasUserReviewedAppointmentAsync(userId, appointmentId);

        // Assert
        result.Should().BeTrue();
    }
}