using CozyCafe.Application.Exceptions;
using CozyCafe.Application.Interfaces.ForRerository.ForUser;
using CozyCafe.Application.Services.ForUser; 
using CozyCafe.Models.Domain.ForUser;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace CozyCafe.Tests.Services
{
    public class ReviewServiceTests
    {
        private readonly Mock<IReviewRepository> _reviewRepositoryMock;
        private readonly Mock<ILogger<ReviewService>> _loggerMock;
        private readonly ReviewService _service;

        public ReviewServiceTests()
        {
            _reviewRepositoryMock = new Mock<IReviewRepository>();
            _loggerMock = new Mock<ILogger<ReviewService>>();
            _service = new ReviewService(_reviewRepositoryMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task GetByMenuItemIdAsync_ReturnsReviews_WhenFound()
        {
            // Arrange
            var menuItemId = 1;
            var reviews = new List<Review>
            {
                new Review { Id = 1, Rating = 5, UserId = "u1", MenuItemId = menuItemId }
            };

            _reviewRepositoryMock
                .Setup(r => r.GetByMenuItemIdAsync(menuItemId))
                .ReturnsAsync(reviews);

            // Act
            var result = await _service.GetByMenuItemIdAsync(menuItemId);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(menuItemId, result.First().MenuItemId);
        }

        [Fact]
        public async Task GetByMenuItemIdAsync_ThrowsNotFound_WhenNoReviews()
        {
            // Arrange
            var menuItemId = 1;
            _reviewRepositoryMock
                .Setup(r => r.GetByMenuItemIdAsync(menuItemId))
                .ReturnsAsync(new List<Review>());

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => _service.GetByMenuItemIdAsync(menuItemId));
        }

        [Fact]
        public async Task GetByUserIdAsync_ReturnsReviews_WhenFound()
        {
            // Arrange
            var userId = "u123";
            var reviews = new List<Review>
            {
                new Review { Id = 1, Rating = 4, UserId = userId, MenuItemId = 2 }
            };

            _reviewRepositoryMock
                .Setup(r => r.GetByUserIdAsync(userId))
                .ReturnsAsync(reviews);

            // Act
            var result = await _service.GetByUserIdAsync(userId);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(userId, result.First().UserId);
        }

        [Fact]
        public async Task GetByUserIdAsync_ThrowsNotFound_WhenNoReviews()
        {
            // Arrange
            var userId = "u123";
            _reviewRepositoryMock
                .Setup(r => r.GetByUserIdAsync(userId))
                .ReturnsAsync(new List<Review>());

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => _service.GetByUserIdAsync(userId));
        }
    }
}
