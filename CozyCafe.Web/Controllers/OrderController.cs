using System.Security.Claims;
using AutoMapper;
using CozyCafe.Application.Interfaces.ForServices;
using CozyCafe.Models.Domain;
using CozyCafe.Models.DTO;
using Microsoft.AspNetCore.Mvc;

namespace CozyCafe.Web.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;
        public OrderController(IOrderService orderService, IMapper mapper)
        {
            _orderService = orderService;
            _mapper = mapper;   
        }
        // GET: всі замовлення користувача
        public async Task<IActionResult> MyOrders()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var orders = await _orderService.GetByUserIdAsync(userId);
            var dto = _mapper.Map<IEnumerable<OrderDto>>(orders);
            return View("UserOrders", dto);
        }

        // GET: повна інформація про замовлення
        public async Task<IActionResult> Details(int id)
        {
            var order = await _orderService.GetFullOrderAsync(id);
            if (order == null) 
                return NotFound();

            var dto = _mapper.Map<OrderDto>(order);

            return View("OrderDetails", dto);
        }

        // POST: Додати позицію до замовлення
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddItem(int orderId, OrderItem item)
        {
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
