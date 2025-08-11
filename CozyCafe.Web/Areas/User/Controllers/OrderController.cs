using System.Security.Claims;
using AutoMapper;
using CozyCafe.Application.Interfaces.Logging;
using CozyCafe.Models.DTO.ForUser;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CozyCafe.Web.Areas.User.Controllers
{
    [Area("User")]
    [Authorize]
    [Route("User/[controller]/[action]")]
    public class OrderController : Controller
    {
        private readonly ILoggerService _logger;
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;
        private readonly IMenuItemService _menuItemService;
        private readonly IMenuItemOptionService _menuItemOptionService;
        private readonly ICartService _cartService;

        public OrderController(
            ILoggerService logger,
            IOrderService orderService,
            IMapper mapper,
            IMenuItemService menuItemService,
            IMenuItemOptionService menuItemOptionService,
            ICartService cartService)
        {
            _logger = logger;
            _orderService = orderService;
            _mapper = mapper;
            _menuItemService = menuItemService;
            _menuItemOptionService = menuItemOptionService;
            _cartService = cartService;
        }

        public async Task<IActionResult> Index()
        {
            var orders = await _orderService.GetAllAsync();
            var dtos = _mapper.Map<IEnumerable<OrderDto>>(orders);
            return View(dtos);
        }

        public async Task<IActionResult> Details(int id)
        {
            var order = await _orderService.GetFullOrderAsync(id);
            if (order == null)
            {
                var userName = User.Identity?.Name ?? "Анонім";
                _logger.LogWarning($"{userName}: Замовлення з Id={id} не знайдено");
                return NotFound();
            }

            var dto = _mapper.Map<OrderDto>(order);
            var menuItems = await _menuItemService.GetAllAsync();
            ViewBag.MenuItems = new SelectList(menuItems, "Id", "Name");

            return View("OrderDetails", dto);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new CreateOrderDto
            {
                Items = new List<CreateOrderItemDto>()
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateOrderDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userName = User.Identity?.Name ?? "Анонім";

            if (userId == null)
            {
                _logger.LogWarning($"{userName}: Спроба створення замовлення неавторизованим користувачем");
                return Unauthorized();
            }

            var cart = await _cartService.GetByUserIdAsync(userId);
            if (cart == null || cart.Items == null || !cart.Items.Any())
            {
                _logger.LogWarning($"{userName}: Спроба створення замовлення з порожнім кошиком");
                ModelState.AddModelError("", "Кошик порожній");
                return View(dto);
            }

            var order = new Order
            {
                UserId = userId,
                OrderedAt = DateTime.UtcNow,
                Status = Order.OrderStatus.Pending,
                Items = new List<OrderItem>()
            };

            decimal total = 0;

            foreach (var cartItem in cart.Items)
            {
                var menuItem = await _menuItemService.GetByIdAsync(cartItem.MenuItemId);
                if (menuItem == null)
                {
                    _logger.LogWarning($"{userName}: У кошику є неіснуючий товар з MenuItemId={cartItem.MenuItemId}");
                    continue;
                }

                var orderItem = new OrderItem
                {
                    MenuItemId = cartItem.MenuItemId,
                    Quantity = cartItem.Quantity,
                    Price = menuItem.Price * cartItem.Quantity,
                    SelectedOptions = new List<OrderItemOption>()
                };

                total += orderItem.Price;
                order.Items.Add(orderItem);
            }

            order.Total = total;

            await _orderService.AddAsync(order);
            await _cartService.ClearCartAsync(userId);

            _logger.LogInfo(userName, $"Оформлено нове замовлення на суму {order.Total}₴");

            return RedirectToAction("Index", "Home");
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var order = await _orderService.GetByIdAsync(id);
            if (order == null)
            {
                var userName = User.Identity?.Name ?? "Анонім";
                _logger.LogWarning($"{userName}: Спроба редагування неіснуючого замовлення з Id={id}");
                return NotFound();
            }

            var dto = _mapper.Map<OrderDto>(order);
            dto.UserId = order.UserId;

            return View(dto);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, OrderDto dto)
        {
            var userName = User.Identity?.Name ?? "Анонім";

            if (id != dto.Id)
            {
                _logger.LogWarning($"{userName}: Невідповідність Id в URL ({id}) та в моделі ({dto.Id}) при редагуванні замовлення");
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                _logger.LogWarning($"{userName}: Невірні дані при редагуванні замовлення з Id={id}");
                return View(dto);
            }

            await _orderService.UpdateOrderStatusAsync(id, dto.Status);
            _logger.LogInfo(userName, $"Адмін змінив статус замовлення #{id} на '{dto.Status}'");

            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var order = await _orderService.GetByIdAsync(id);
            if (order == null)
            {
                var userName = User.Identity?.Name ?? "Анонім";
                _logger.LogWarning($"{userName}: Спроба видалення неіснуючого замовлення з Id={id}");
                return NotFound();
            }

            var dto = _mapper.Map<OrderDto>(order);
            return View(dto);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userName = User.Identity?.Name ?? "Анонім";

            var order = await _orderService.GetByIdAsync(id);
            if (order != null)
            {
                await _orderService.DeleteAsync(id);
                _logger.LogInfo(userName, $"Адмін видалив замовлення #{id}");
            }
            else
            {
                _logger.LogWarning($"{userName}: Спроба видалення неіснуючого замовлення з Id={id}");
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> MyOrders()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return Unauthorized();

            var orders = await _orderService.GetByUserIdAsync(userId);
            var dtos = _mapper.Map<IEnumerable<OrderDto>>(orders);
            return View("UserOrders", dtos);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddItem(int orderId, int MenuItemId, int Quantity)
        {
            var userName = User.Identity?.Name ?? "Анонім";

            var menuItem = await _menuItemService.GetByIdAsync(MenuItemId);
            if (menuItem == null)
            {
                _logger.LogWarning($"{userName}: Спроба додати до замовлення #{orderId} неіснуючий товар з MenuItemId={MenuItemId}");
                return NotFound();
            }

            var item = new OrderItem
            {
                MenuItemId = MenuItemId,
                Quantity = Quantity,
                Price = menuItem.Price * Quantity,
                SelectedOptions = new List<OrderItemOption>()
            };

            await _orderService.AddOrderItemAsync(orderId, item);
            _logger.LogInfo(userName, $"Додано товар з Id={MenuItemId} у замовлення #{orderId}");

            return RedirectToAction("Details", new { id = orderId });
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateStatus(int orderId, string newStatus)
        {
            var userName = User.Identity?.Name ?? "Анонім";

            try
            {
                await _orderService.UpdateOrderStatusAsync(orderId, newStatus);
                _logger.LogInfo(userName, $"Адмін оновив статус замовлення #{orderId} на '{newStatus}'");
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"{userName}: Помилка оновлення статусу замовлення #{orderId}: {ex.Message}");
            }

            return RedirectToAction("Index", "Order", new { area = "User" });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOption(int orderId, int orderItemId, OrderItemOption option)
        {
            var userName = User.Identity?.Name ?? "Анонім";

            try
            {
                await _orderService.AddOptionToOrderItemAsync(orderId, orderItemId, option);
                _logger.LogInfo(userName, $"Додано опцію до пункту замовлення #{orderItemId} у замовленні #{orderId}");
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"{userName}: Помилка додавання опції до пункту замовлення #{orderItemId} у замовленні #{orderId}: {ex.Message}");
            }

            return RedirectToAction("Details", new { id = orderId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveItem(int orderId, int orderItemId)
        {
            var userName = User.Identity?.Name ?? "Анонім";

            try
            {
                await _orderService.RemoveOrderItemAsync(orderId, orderItemId);
                _logger.LogInfo(userName, $"Видалено пункт замовлення #{orderItemId} із замовлення #{orderId}");
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"{userName}: Помилка видалення пункту замовлення #{orderItemId} із замовлення #{orderId}: {ex.Message}");
            }

            return RedirectToAction("Details", new { id = orderId });
        }
    }
}
