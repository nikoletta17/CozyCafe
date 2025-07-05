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

        public OrderController(
            IOrderService orderService,
            IMapper mapper,
            IMenuItemService menuItemService,
            IDiscountService discountService,
            IMenuItemOptionService menuItemOptionService)
        {
            _orderService = orderService;
            _mapper = mapper;
            _menuItemService = menuItemService;
            _discountService = discountService;
            _menuItemOptionService = menuItemOptionService;
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
            if (!ModelState.IsValid)
            {
                return View(dto);
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return Unauthorized();

            var order = new Order
            {
                UserId = userId,
                OrderedAt = DateTime.UtcNow,
                Status = Order.OrderStatus.Pending,
                Items = new List<OrderItem>()
            };

            // Якщо є знижка — знайти по DiscountCode
            if (!string.IsNullOrEmpty(dto.DiscountCode))
            {
                var discount = await _discountService.GetByCodeAsync(dto.DiscountCode); // реалізуй цей метод
                if (discount != null)
                {
                    order.DiscountId = discount.Id;
                }
            }

            decimal total = 0;

            foreach (var dtoItem in dto.Items)
            {
                var menuItem = await _menuItemService.GetByIdAsync(dtoItem.MenuItemId);
                if (menuItem == null)
                    continue;

                var orderItem = new OrderItem
                {
                    MenuItemId = dtoItem.MenuItemId,
                    Quantity = dtoItem.Quantity,
                    Price = menuItem.Price * dtoItem.Quantity,
                    SelectedOptions = new List<OrderItemOption>()
                };

                total += orderItem.Price;

                foreach (var optionId in dtoItem.SelectedOptionIds)
                {
                    var option = await _menuItemOptionService.GetByIdAsync(optionId);
                    if (option != null)
                    {
                        total += option.ExtraPrice ?? 0;

                        orderItem.SelectedOptions.Add(new OrderItemOption
                        {
                            MenuItemOptionId = option.Id
                        });
                    }
                }

                order.Items.Add(orderItem);
            }

            order.Total = total;

            await _orderService.AddAsync(order);

            return RedirectToAction("Index", "Home"); // або інша сторінка
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
