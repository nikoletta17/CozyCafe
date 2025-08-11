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
        _logger.LogInformation("Знайдено {Count} відгуків для меню {MenuItemId}", reviews.Count(), menuItemId);
        return reviews;
    }

    public async Task<IEnumerable<Review>> GetByUserIdAsync(string userId)
    {
        _logger.LogInformation("Отримання відгуків користувача {UserId}", userId);
        var reviews = await _reviewRepository.GetByUserIdAsync(userId);
        _logger.LogInformation("Знайдено {Count} відгуків користувача {UserId}", reviews.Count(), userId);
        return reviews;
    }
}
