using AutoMapper;
using CozyCafe.Models.DTO.ForUser;
using CozyCafe.Web.Areas.User.Controllers.Generic_Controller;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CozyCafe.Web.Areas.User.Controllers
{
    /// <summary>
    /// (UA) Контролер для роботи з відгуками.
    /// - Наслідує GenericController<Review>.
    /// - ByMenuItem(int menuItemId): Показує відгуки для конкретного товару.
    /// - ByUser(string userId): Показує відгуки конкретного користувача.
    /// - Create(): GET/POST – створення нового відгуку; враховує MenuItemId та поточного користувача.
    /// - All(): GET – показує всі відгуки, доступно анонімно.
    /// - Використовує IReviewService, IMenuItemService, IMapper та ILogger для бізнес-логіки та логування.
    /// 
    /// (EN) Controller for handling reviews.
    /// - Inherits GenericController<Review>.
    /// - ByMenuItem(int menuItemId): Displays reviews for a specific menu item.
    /// - ByUser(string userId): Displays reviews for a specific user.
    /// - Create(): GET/POST – creates a new review; considers MenuItemId and current user.
    /// - All(): GET – shows all reviews, anonymously accessible.
    /// - Uses IReviewService, IMenuItemService, IMapper, and ILogger for business logic and logging.
    /// </summary>

    [Area("User")]
    [Authorize]
    [Route("User/[controller]/[action]")]
    public class ReviewController : GenericController<Review>
    {
        private readonly IReviewService _reviewService;
        private readonly IMapper _mapper;
        private readonly IMenuItemService _menuItemService;
        private readonly ILogger<ReviewController> _logger;

        public ReviewController(IReviewService reviewService, IMapper mapper, IMenuItemService menuItemService, ILogger<ReviewController> logger)
            : base(reviewService)
        {
            _reviewService = reviewService;
            _mapper = mapper;
            _menuItemService = menuItemService;
            _logger = logger;
        }

        // Всі відгуки для певного MenuItem
        public async Task<IActionResult> ByMenuItem(int menuItemId)
        {
            _logger.LogInformation("Отримання відгуків для MenuItemId = {MenuItemId}", menuItemId);

            var reviews = await _reviewService.GetByMenuItemIdAsync(menuItemId);
            var dto = _mapper.Map<IEnumerable<ReviewDto>>(reviews);

            ViewBag.MenuItemId = menuItemId;

            _logger.LogInformation("Отримано {Count} відгуків для MenuItemId = {MenuItemId}", dto.Count(), menuItemId);

            return View("ReviewsByMenuItem", dto);
        }

        // Всі відгуки певного користувача
        public async Task<IActionResult> ByUser(string userId)
        {
            _logger.LogInformation("Отримання відгуків користувача {UserId}", userId);

            var reviews = await _reviewService.GetByUserIdAsync(userId);
            var dto = _mapper.Map<IEnumerable<ReviewDto>>(reviews);

            _logger.LogInformation("Отримано {Count} відгуків користувача {UserId}", dto.Count(), userId);

            return View("ReviewsByUser", dto);
        }

        [HttpGet("Create")]
        public async Task<IActionResult> Create(int? menuItemId)
        {
            _logger.LogInformation("Початок створення відгуку. MenuItemId = {MenuItemId}", menuItemId);

            var menuItems = await _menuItemService.GetFilteredAsync(new MenuItemFilterModel());

            ViewBag.MenuItems = menuItems.Select(mi => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
            {
                Value = mi.Id.ToString(),
                Text = mi.Name,
                Selected = (mi.Id == menuItemId)
            }).ToList();

            var model = new CreateReviewDto();

            if (menuItemId.HasValue)
                model.MenuItemId = menuItemId.Value;

            return View(model);
        }

        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateReviewDto dto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Невірна модель при створенні відгуку для MenuItemId = {MenuItemId}", dto.MenuItemId);

                var menuItems = await _menuItemService.GetFilteredAsync(new MenuItemFilterModel());
                ViewBag.MenuItems = menuItems.Select(mi => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
                {
                    Value = mi.Id.ToString(),
                    Text = mi.Name,
                    Selected = (mi.Id == dto.MenuItemId)
                }).ToList();

                return View(dto);
            }

            var review = _mapper.Map<Review>(dto);
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                _logger.LogWarning("Не вдалося визначити користувача при створенні відгуку");

                ModelState.AddModelError("", "Не вдалося визначити користувача.");

                var menuItems = await _menuItemService.GetFilteredAsync(new MenuItemFilterModel());
                ViewBag.MenuItems = menuItems.Select(mi => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
                {
                    Value = mi.Id.ToString(),
                    Text = mi.Name,
                    Selected = (mi.Id == dto.MenuItemId)
                }).ToList();

                return View(dto);
            }

            review.UserId = userId;
            review.CreatedAt = DateTime.UtcNow;

            await _reviewService.AddAsync(review);

            _logger.LogInformation("Користувач {UserId} створив відгук для MenuItemId = {MenuItemId}", userId, dto.MenuItemId);

            return RedirectToAction("ByMenuItem", new { menuItemId = dto.MenuItemId });
        }

        [AllowAnonymous]
        public async Task<IActionResult> All()
        {
            _logger.LogInformation("Отримання всіх відгуків (доступно анонімно)");

            var reviews = await _reviewService.GetAllAsync();
            var dto = _mapper.Map<IEnumerable<ReviewDto>>(reviews);

            return View("AllReviews", dto);
        }
    }
}
