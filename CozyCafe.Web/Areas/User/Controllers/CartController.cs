using AutoMapper;
using CozyCafe.Models.DTO.ForUser;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CozyCafe.Web.Areas.User.Controllers
{
    /// <summary>
    /// (UA) Контролер для управління кошиком користувача в зоні User.
    /// - Доступ обмежено авторизованими користувачами ([Authorize]).
    /// - Index(): Показує поточний кошик користувача; повертає порожній, якщо кошик пустий.
    /// - AddOrUpdateItem(): POST – додає або оновлює товар у кошику з обраними опціями.
    /// - UpdateItemQuantity(): POST – змінює кількість конкретного товару.
    /// - RemoveItem(): POST – видаляє товар з кошика.
    /// - Clear(): POST – очищає весь кошик користувача.
    /// - Використовує ICartService, IMenuItemService, IMenuItemOptionService, UserManager та ILogger для бізнес-логіки і логування.
    /// 
    /// (EN) Controller for managing the user's cart in the User area.
    /// - Access restricted to authorized users ([Authorize]).
    /// - Index(): Displays the current user's cart; returns empty cart if none exists.
    /// - AddOrUpdateItem(): POST – adds or updates an item in the cart with selected options.
    /// - UpdateItemQuantity(): POST – updates the quantity of a specific cart item.
    /// - RemoveItem(): POST – removes an item from the cart.
    /// - Clear(): POST – clears the entire user's cart.
    /// - Uses ICartService, IMenuItemService, IMenuItemOptionService, UserManager, and ILogger for business logic and logging.
    /// </summary>

    [Area("User")]
    [Authorize]
    [Route("User/[controller]/[action]")]
    public class CartController : Controller
    {
        private readonly ICartService _cartService;
        private readonly IMenuItemService _menuItemService;
        private readonly IMenuItemOptionService _menuItemOptionService;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<CartController> _logger;

        public CartController(
            ICartService cartService,
            IMenuItemService menuItemService,
            IMenuItemOptionService menuItemOptionService,
            IMapper mapper,
            UserManager<ApplicationUser> userManager,
            ILogger<CartController> logger)
        {
            _cartService = cartService;
            _menuItemService = menuItemService;
            _menuItemOptionService = menuItemOptionService;
            _mapper = mapper;
            _userManager = userManager;
            _logger = logger;
        }

        // GET: /Cart
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                _logger.LogWarning("Неавторизований доступ до Cart/Index");
                return Unauthorized();
            }

            var cart = await _cartService.GetByUserIdAsync(userId);
            if (cart == null)
            {
                _logger.LogInformation("Користувач {UserId} має порожній кошик", userId);
                return View(new CartDto { Items = new List<CartItemDto>() });
            }

            var dto = await _cartService.GetCartAsync(userId);
            _logger.LogInformation("Користувач {UserId} переглядає кошик із {ItemCount} товар(ами)", userId, dto.Items.Count);
            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrUpdateItem(int menuItemId, int quantity, int[] selectedOptionIds)
        {
            var userId = _userManager.GetUserId(User);
            if (userId == null)
            {
                _logger.LogWarning("Неавторизований запит AddOrUpdateItem");
                return Unauthorized();
            }

            await _cartService.AddOrUpdateItemAsync(userId, menuItemId, quantity, selectedOptionIds ?? new int[0]);
            _logger.LogInformation("Користувач {UserId} додав/оновив товар {MenuItemId} з кількістю {Quantity}", userId, menuItemId, quantity);
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateItemQuantity(int menuItemId, int quantity)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                _logger.LogWarning("Неавторизований запит UpdateItemQuantity");
                return Unauthorized();
            }

            await _cartService.UpdateItemQuantityAsync(userId, menuItemId, quantity);
            _logger.LogInformation("Користувач {UserId} оновив кількість товару {MenuItemId} до {Quantity}", userId, menuItemId, quantity);
            return RedirectToAction(nameof(Index));
        }

        // Видалити товар з кошика
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveItem(int menuItemId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                _logger.LogWarning("Неавторизований запит RemoveItem");
                return Unauthorized();
            }

            await _cartService.RemoveCartItemAsync(userId, menuItemId);
            _logger.LogInformation("Користувач {UserId} видалив товар {MenuItemId} з кошика", userId, menuItemId);
            return RedirectToAction(nameof(Index));
        }

        // Очистити весь кошик
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Clear()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                _logger.LogWarning("Неавторизований запит ClearCart");
                return Unauthorized();
            }

            await _cartService.ClearCartAsync(userId);
            _logger.LogInformation("Користувач {UserId} очистив кошик", userId);
            return RedirectToAction(nameof(Index));
        }
    }
}
