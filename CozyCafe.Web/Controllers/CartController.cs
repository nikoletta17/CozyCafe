using System.Security.Claims;
using AutoMapper;
using CozyCafe.Application.Interfaces.ForServices;
using CozyCafe.Models.DTO;
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

        // POST: /Cart/Add?menuItemId=1&quantity=2
        [HttpPost]
        public async Task<IActionResult> Add(int menuItemId, int quantity)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();

            await _cartService.AddOrUpdateItemAsync(userId, menuItemId, quantity);
            return RedirectToAction(nameof(Index));
        }

        // POST: /Cart/Update?menuItemId=1&quantity=3
        [HttpPost]
        public async Task<IActionResult> Update(int menuItemId, int quantity)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();

            await _cartService.UpdateItemQuantityAsync(userId, menuItemId, quantity);
            return RedirectToAction(nameof(Index));
        }

        // POST: /Cart/Remove?menuItemId=1
        [HttpPost]
        public async Task<IActionResult> Remove(int menuItemId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();

            await _cartService.RemoveItemAsync(userId, menuItemId);
            return RedirectToAction(nameof(Index));
        }

        // POST: /Cart/Clear
        [HttpPost]
        public async Task<IActionResult> Clear()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();

            await _cartService.ClearCartAsync(userId);
            return RedirectToAction(nameof(Index));
        }
    }
}
