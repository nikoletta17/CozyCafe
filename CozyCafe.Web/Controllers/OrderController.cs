using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using CozyCafe.Application.Interfaces.ForServices.ForAdmin;
using CozyCafe.Application.Interfaces.ForServices.ForUser;
using CozyCafe.Models.Domain.Common;
using CozyCafe.Models.DTO.ForUser;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CozyCafe.Web.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;
        private readonly IMenuItemService _menuItemService;

        public OrderController(IOrderService orderService, IMapper mapper, IMenuItemService menuItemService)
        {
            _orderService = orderService;
            _mapper = mapper;
            _menuItemService = menuItemService;
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
            return View();
        }

        // POST: /Order/Create — створити замовлення
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Order order)
        {
            if (ModelState.IsValid)
            {
                order.OrderedAt = DateTime.UtcNow;
                order.Status = Order.OrderStatus.Pending;
                await _orderService.AddAsync(order);
                return RedirectToAction(nameof(Index));
            }
            return View(order);
        }

        // GET: /Order/Edit/5 — форма редагування
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var order = await _orderService.GetByIdAsync(id);
            if (order == null)
                return NotFound();
            return View(order);
        }

        // POST: /Order/Edit/5 — оновити замовлення
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
            var item = new OrderItem
            {
                MenuItemId = MenuItemId,
                Quantity = Quantity
            };
            await _orderService.AddOrderItemAsync(orderId, item);
            return RedirectToAction("Details", new { id = orderId });
        }

        // POST: Оновити статус замовлення
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
