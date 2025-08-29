using CozyCafe.Application.Exceptions;
using CozyCafe.Application.Interfaces.ForRerository.ForUser;
using CozyCafe.Application.Services.ForUser; 
using CozyCafe.Models.Domain.ForUser;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace CozyCafe.Tests.Services
{
    /// <summary>
    /// (UA) Тести для сервісу ReviewService у CozyCafe.
    /// Перевіряють основні методи сервісу:
    /// - GetByMenuItemIdAsync: повертає відгуки за Id меню; перевірка випадків, коли відгуки існують та коли їх немає (викидає NotFoundException).
    /// - GetByUserIdAsync: повертає відгуки користувача за Id; перевірка випадків з наявними відгуками та з відсутніми (викидає NotFoundException).
    /// Використовуються мок-репозиторії IReviewRepository та логер ILogger для ізоляції тестів.
    ///
    /// (EN) Tests for the ReviewService in CozyCafe.
    /// Verify main service methods:
    /// - GetByMenuItemIdAsync: returns reviews by menu item ID; checks both cases when reviews exist and when none exist (throws NotFoundException).
    /// - GetByUserIdAsync: returns reviews by user ID; checks both cases with existing reviews and without (throws NotFoundException).
    /// Uses mocked IReviewRepository and ILogger to isolate the tests.
    /// </summary>

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
