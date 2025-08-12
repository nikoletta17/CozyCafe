using CozyCafe.Application.Exceptions;
using CozyCafe.Application.Interfaces.ForRerository.ForUser;
using CozyCafe.Application.Interfaces.ForServices.ForUser;
using CozyCafe.Application.Services.Generic_Service;
using CozyCafe.Models.Domain.ForUser;
using Microsoft.Extensions.Logging;

public class ReviewService : Service<Review>, IReviewService
{
    private readonly IReviewRepository _reviewRepository;
    private readonly ILogger<ReviewService> _logger;

    public ReviewService(IReviewRepository reviewRepository, ILogger<ReviewService> logger) : base(reviewRepository)
    {
        _reviewRepository = reviewRepository;
        _logger = logger;
    }

    public async Task<IEnumerable<Review>> GetByMenuItemIdAsync(int menuItemId)
    {
        _logger.LogInformation("Отримання відгуків для меню {MenuItemId}", menuItemId);

        var reviews = await _reviewRepository.GetByMenuItemIdAsync(menuItemId);

        if (reviews == null || !reviews.Any())
        {
            _logger.LogWarning("Відгуки для меню {MenuItemId} не знайдені", menuItemId);
            throw new NotFoundException("Review", $"MenuItemId={menuItemId}");
        }

        _logger.LogInformation("Знайдено {Count} відгуків для меню {MenuItemId}", reviews.Count(), menuItemId);
        return reviews;
    }

    public async Task<IEnumerable<Review>> GetByUserIdAsync(string userId)
    {
        _logger.LogInformation("Отримання відгуків користувача {UserId}", userId);

        var reviews = await _reviewRepository.GetByUserIdAsync(userId);

        if (reviews == null || !reviews.Any())
        {
            _logger.LogWarning("Відгуки користувача {UserId} не знайдені", userId);
            throw new NotFoundException("Review", $"UserId={userId}");
        }

        _logger.LogInformation("Знайдено {Count} відгуків користувача {UserId}", reviews.Count(), userId);
        return reviews;
    }
}
