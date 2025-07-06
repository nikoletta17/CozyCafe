using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using CozyCafe.Application.Interfaces.ForServices.ForAdmin;
using CozyCafe.Application.Interfaces.ForServices.ForUser;
using CozyCafe.Application.Services.ForAdmin;
using CozyCafe.Models.Domain.Common;
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
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;
        private readonly IMenuItemService _menuItemService;
        private readonly IDiscountService _discountService;
        private readonly IMenuItemOptionService _menuItemOptionService;
        private readonly ICartService _cartService;

        public OrderController(
            IOrderService orderService,
            IMapper mapper,
            IMenuItemService menuItemService,
            IDiscountService discountService,
            IMenuItemOptionService menuItemOptionService,
            ICartService cartService) // додано
        {
            _orderService = orderService;
            _mapper = mapper;
            _menuItemService = menuItemService;
            _discountService = discountService;
            _menuItemOptionService = menuItemOptionService;
            _cartService = cartService;
        }


        // GET: /Order/Index — всі замовлення (для адміністратора, наприклад)
        public async Task<IActionResult> Index()
        {
            var orders = await _orderService.GetAllAsync();
            var dtos = _mapper.Map<IEnumerable<OrderDto>>(orders);
            return View(dtos);
        }

        // GET: /Order/Details/5 — повна інформація про замовлення
        public async Task<IActionResult> Details(int id)
        {
            var order = await _orderService.GetFullOrderAsync(id);
            if (order == null)
                return NotFound();

            var dto = _mapper.Map<OrderDto>(order);
            var menuItems = await _menuItemService.GetAllAsync();
            ViewBag.MenuItems = new SelectList(menuItems, "Id", "Name");

            return View("OrderDetails", dto);
        }

        // GET: /Order/Create — форма створення замовлення
        [HttpGet]
        public IActionResult Create()
        {
            return View(new CreateOrderDto
            {
                Items = new List<CreateOrderItemDto>() // додано
            });
        }



        // POST: /Order/Create — створити замовлення
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateOrderDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return Unauthorized();

            var cart = await _cartService.GetByUserIdAsync(userId);
            if (cart == null || cart.Items == null || !cart.Items.Any())
            {
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

            // Пошук знижки
            if (!string.IsNullOrEmpty(dto.DiscountCode))
            {
                var discount = await _discountService.GetByCodeAsync(dto.DiscountCode);
                if (discount != null)
                {
                    order.DiscountId = discount.Id;
                }
            }

            decimal total = 0;

            foreach (var cartItem in cart.Items)
            {
                var menuItem = await _menuItemService.GetByIdAsync(cartItem.MenuItemId);
                if (menuItem == null)
                    continue;

                var orderItem = new OrderItem
                {
                    MenuItemId = cartItem.MenuItemId,
                    Quantity = cartItem.Quantity,
                    Price = menuItem.Price * cartItem.Quantity,
                    SelectedOptions = new List<OrderItemOption>() // Якщо треба, додай логіку для опцій
                };

                total += orderItem.Price;
                order.Items.Add(orderItem);
            }


            order.Total = total;

            await _orderService.AddAsync(order);
            await _cartService.ClearCartAsync(userId); // Очистити кошик після замовлення

            return RedirectToAction("Index", "Home"); // або інша сторінка
        }





        // GET: /Order/Edit/5 — форма редагування
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var order = await _orderService.GetByIdAsync(id);
            if (order == null)
                return NotFound();
            return View(order);
        }

        // POST: /Order/Edit/5 — оновити замовлення
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Order order)
        {
            if (id != order.Id)
                return BadRequest();

            if (ModelState.IsValid)
            {
                await _orderService.UpdateAsync(order);
                return RedirectToAction(nameof(Index));
            }
            return View(order);
        }

        // GET: /Order/Delete/5 — підтвердження видалення
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var order = await _orderService.GetByIdAsync(id);
            if (order == null)
                return NotFound();

            var dto = _mapper.Map<OrderDto>(order);
            return View(dto);
        }

        // POST: /Order/Delete/5 — видалення замовлення
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var order = await _orderService.GetByIdAsync(id);
            if (order != null)
            {
                await _orderService.DeleteAsync(id);
            }
            return RedirectToAction(nameof(Index));
        }

        // Спеціальні методи, що були у тебе раніше:

        // GET: всі замовлення користувача (MyOrders)
        public async Task<IActionResult> MyOrders()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return Unauthorized();

            var orders = await _orderService.GetByUserIdAsync(userId);
            var dtos = _mapper.Map<IEnumerable<OrderDto>>(orders);
            return View("UserOrders", dtos);
        }

        // POST: Додати позицію до замовлення
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddItem(int orderId, int MenuItemId, int Quantity)
        {
            var menuItem = await _menuItemService.GetByIdAsync(MenuItemId);
            if (menuItem == null)
                return NotFound();

            var item = new OrderItem
            {
                MenuItemId = MenuItemId,
                Quantity = Quantity,
                Price = menuItem.Price * Quantity,
                SelectedOptions = new List<OrderItemOption>() // якщо потрібні будуть у майбутньому
            };

            await _orderService.AddOrderItemAsync(orderId, item);
            return RedirectToAction("Details", new { id = orderId });
        }


        // POST: Оновити статус замовлення
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateStatus(int orderId, string newStatus)
        {
            await _orderService.UpdateOrderStatusAsync(orderId, newStatus);
            return RedirectToAction("Details", new { id = orderId });
        }

        // POST: Додати опцію до пункту замовлення
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOption(int orderId, int orderItemId, OrderItemOption option)
        {
            await _orderService.AddOptionToOrderItemAsync(orderId, orderItemId, option);
            return RedirectToAction("Details", new { id = orderId });
        }

        // POST: Видалити опцію з пункту замовлення
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveOption(int orderId, int orderItemId, int optionId)
        {
            await _orderService.RemoveOrderItemOptionAsync(orderId, orderItemId, optionId);
            return RedirectToAction("Details", new { id = orderId });
        }
    }
}
