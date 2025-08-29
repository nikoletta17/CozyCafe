using AutoMapper;
using CozyCafe.Application.Interfaces.ForServices.ForAdmin;
using CozyCafe.Application.Interfaces.ForServices.ForUser;
using CozyCafe.Models.Domain.ForUser;
using CozyCafe.Models.DTO.Admin;
using CozyCafe.Models.DTO.ForUser;
using CozyCafe.Web.Areas.User.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System.Security.Claims;

namespace CozyCafe.Tests.Controllers
{
    /// <summary>
    /// (UA) Тести для контролера ReviewController у CozyCafe.
    /// Перевіряють основні дії контролера:
    /// - ByMenuItem: отримання відгуків для конкретного меню та передача їх у View.
    /// - ByUser: отримання відгуків конкретного користувача та передача у View.
    /// - Create (POST): додавання нового відгуку з перевіркою валідності моделі.
    /// - All: отримання всіх відгуків та передача у View.
    /// Використовуємо мок-сервіси для IReviewService, IMapper та IMenuItemService, а також перевіряємо коректність ViewResult, RedirectToActionResult та моделі.
    ///
    /// (EN) Tests for the ReviewController in CozyCafe.
    /// Verify main controller actions:
    /// - ByMenuItem: fetch reviews for a menu item and return them to the View.
    /// - ByUser: fetch reviews for a specific user and return to the View.
    /// - Create (POST): add a new review with model validation.
    /// - All: fetch all reviews and return to the View.
    /// Uses mocked services for IReviewService, IMapper, and IMenuItemService, and asserts correct ViewResult, RedirectToActionResult, and model.
    /// </summary>

    public class ReviewControllerTests
    {
        private readonly Mock<IReviewService> _reviewServiceMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IMenuItemService> _menuItemServiceMock;
        private readonly Mock<ILogger<ReviewController>> _loggerMock;
        private readonly ReviewController _controller;

        public ReviewControllerTests()
        {
            _reviewServiceMock = new Mock<IReviewService>();
            _mapperMock = new Mock<IMapper>();
            _menuItemServiceMock = new Mock<IMenuItemService>();
            _loggerMock = new Mock<ILogger<ReviewController>>();

            _controller = new ReviewController(
                _reviewServiceMock.Object,
                _mapperMock.Object,
                _menuItemServiceMock.Object,
                _loggerMock.Object
            );
        }

        [Fact]
        public async Task ByMenuItem_ReturnsViewWithMappedReviews()
        {
            // Arrange
            int menuItemId = 1;
            var reviews = new List<Review> { new Review { Id = 1, Rating = 5, UserId = "u1" } };
            var dtoList = new List<ReviewDto> { new ReviewDto { Id = 1, Rating = 5, UserFullName = "Test User" } };

            _reviewServiceMock.Setup(s => s.GetByMenuItemIdAsync(menuItemId))
                              .ReturnsAsync(reviews);
            _mapperMock.Setup(m => m.Map<IEnumerable<ReviewDto>>(reviews))
                       .Returns(dtoList);

            // Act
            var result = await _controller.ByMenuItem(menuItemId) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("ReviewsByMenuItem", result.ViewName);
            Assert.Equal(dtoList, result.Model);
            Assert.Equal(menuItemId, _controller.ViewBag.MenuItemId);
        }

        [Fact]
        public async Task ByUser_ReturnsViewWithMappedReviews()
        {
            // Arrange
            string userId = "user123";
            var reviews = new List<Review> { new Review { Id = 2, Rating = 4, UserId = userId } };
            var dtoList = new List<ReviewDto> { new ReviewDto { Id = 2, Rating = 4, UserFullName = "Some User" } };

            _reviewServiceMock.Setup(s => s.GetByUserIdAsync(userId))
                              .ReturnsAsync(reviews);
            _mapperMock.Setup(m => m.Map<IEnumerable<ReviewDto>>(reviews))
                       .Returns(dtoList);

            // Act
            var result = await _controller.ByUser(userId) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("ReviewsByUser", result.ViewName);
            Assert.Equal(dtoList, result.Model);
        }

        [Fact]
        public async Task Create_Post_InvalidModel_ReturnsViewWithSameDto()
        {
            // Arrange
            var dto = new CreateReviewDto { MenuItemId = 1, Rating = 5 };
            _controller.ModelState.AddModelError("TestError", "Invalid");

            _menuItemServiceMock.Setup(s => s.GetFilteredAsync(It.IsAny<MenuItemFilterModel>()))
                                .ReturnsAsync(new List<MenuItemDto>()); // ✅ Исправлено

            // Act
            var result = await _controller.Create(dto) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(dto, result.Model);
        }

        [Fact]
        public async Task Create_Post_ValidModel_RedirectsToByMenuItem()
        {
            // Arrange
            var dto = new CreateReviewDto { MenuItemId = 5, Rating = 5 };

            var review = new Review { Id = 10, Rating = 5, MenuItemId = 5, UserId = "u123" };

            _mapperMock.Setup(m => m.Map<Review>(dto)).Returns(review);
            _reviewServiceMock.Setup(s => s.AddAsync(It.IsAny<Review>())).Returns(Task.CompletedTask);

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, "u123")
            }, "mock"));

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };

            // Act
            var result = await _controller.Create(dto) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("ByMenuItem", result.ActionName);
            Assert.Equal(dto.MenuItemId, result.RouteValues["menuItemId"]);
        }

        [Fact]
        public async Task All_ReturnsViewWithAllReviews()
        {
            // Arrange
            var reviews = new List<Review> { new Review { Id = 1, Rating = 3, UserId = "u1" } };
            var dtoList = new List<ReviewDto> { new ReviewDto { Id = 1, Rating = 3, UserFullName = "Anon" } };

            _reviewServiceMock.Setup(s => s.GetAllAsync()).ReturnsAsync(reviews);
            _mapperMock.Setup(m => m.Map<IEnumerable<ReviewDto>>(reviews)).Returns(dtoList);

            // Act
            var result = await _controller.All() as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("AllReviews", result.ViewName);
            Assert.Equal(dtoList, result.Model);
        }
    }
}
