using AutoMapper;
using CozyCafe.Models.DTO.ForUser;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CozyCafe.Web.Areas.User.Controllers
{
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
