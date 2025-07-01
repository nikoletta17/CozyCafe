using CozyCafe.Application.Interfaces.ForServices.ForAdmin;
using CozyCafe.Models.DTO.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CozyCafe.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminOrderController : Controller
    {
        private readonly IAdminOrderService _adminOrderService;

        public AdminOrderController(IAdminOrderService adminOrderService)
        {
            _adminOrderService = adminOrderService;
        }

        // GET: AdminOrder/Index?statusFilter=Confirmed&userSearch=ivan
        public async Task<IActionResult> Index(string? statusFilter, string? userSearch)
        {
            var orders = string.IsNullOrEmpty(statusFilter) && string.IsNullOrEmpty(userSearch)
                ? await _adminOrderService.GetAllOrdersAsync()
                : await _adminOrderService.GetOrdersFilteredAsync(statusFilter, userSearch);

            ViewData["CurrentStatusFilter"] = statusFilter;
            ViewData["CurrentUserSearch"] = userSearch;

            return View(orders);
        }

        // GET: AdminOrder/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var order = await _adminOrderService.GetOrderByIdAsync(id);
            if (order == null)
                return NotFound();

            return View(order);
        }

        // POST: AdminOrder/UpdateStatus
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateStatus(UpdateOrderStatusDto dto)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Невірні дані для оновлення статусу.";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                var success = await _adminOrderService.UpdateOrderStatusAsync(dto);
                if (!success)
                {
                    TempData["Error"] = "Не вдалося оновити статус замовлення.";
                }
                else
                {
                    TempData["Success"] = "Статус замовлення успішно оновлено.";
                }
            }
            catch (Exception ex)
            {
                // Тут можна залогувати помилку ex
                TempData["Error"] = "Сталася помилка при оновленні статусу.";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
