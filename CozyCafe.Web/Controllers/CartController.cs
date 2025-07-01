using System.Security.Claims;
using AutoMapper;
using CozyCafe.Application.Interfaces.ForServices.ForUser;
using CozyCafe.Models.Domain;
using CozyCafe.Models.DTO.ForUser;
using Microsoft.AspNetCore.Mvc;

namespace CozyCafe.Web.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService _cartService;
        private readonly IMapper _mapper;
        public CartController(ICartService cartService, IMapper mapper)
        {
            _cartService = cartService;
            _mapper = mapper;
        }

        // GET: /Cart
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();

            var cart = await _cartService.GetByUserIdAsync(userId);
            if (cart == null) return View(new CartDto { Items = new List<CartItemDto>() });

            var dto = _mapper.Map<CartDto>(cart);
            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrUpdateItem(int menuItemId, int quantity)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return Unauthorized();

            var newItem = new CartItem
            {
                MenuItemId = menuItemId,
                Quantity = quantity
            };

            await _cartService.AddOrUpdateCartItemAsync(userId, newItem);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateItemQuantity(int menuItemId, int quantity)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return Unauthorized();

            await _cartService.UpdateItemQuantityAsync(userId, menuItemId, quantity);
            return RedirectToAction(nameof(Index));
        }

        // Видалити товар з кошика
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveItem(int menuItemId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return Unauthorized();

            await _cartService.RemoveCartItemAsync(userId, menuItemId);
            return RedirectToAction(nameof(Index));
        }

        // Очистити весь кошик
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Clear()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return Unauthorized();

            await _cartService.ClearCartAsync(userId);
            return RedirectToAction(nameof(Index));
        }
    }
}
