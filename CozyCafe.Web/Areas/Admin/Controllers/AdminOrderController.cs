using CozyCafe.Application.Interfaces.ForServices.ForAdmin;
using CozyCafe.Models.DTO.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;

namespace CozyCafe.Controllers
{
    /// <summary>
    /// (UA) Контролер для керування замовленнями в адміністративній панелі CozyCafe.
    /// - Доступ тільки для користувачів з роллю Admin.
    /// - Details(int id): отримання детальної інформації про замовлення, підготовка списку можливих статусів для редагування.
    /// - UpdateStatus(UpdateOrderStatusDto dto): оновлення статусу замовлення з перевіркою моделі та логуванням успіху або помилок.
    /// Використовує TempData для передачі повідомлень про успіх або помилки у View.
    /// 
    /// (EN) Controller for managing orders in the CozyCafe admin panel.
    /// - Access restricted to users with the Admin role.
    /// - Details(int id): retrieves detailed information about an order and prepares a list of possible statuses for editing.
    /// - UpdateStatus(UpdateOrderStatusDto dto): updates the order status with model validation and logging of success or errors.
    /// Uses TempData to pass success or error messages to the View.
    /// </summary>

    [Authorize(Roles = "Admin")]
    [Area("Admin")]
    [Route("Admin/[controller]/[action]")]
    public class AdminOrderController : Controller
    {
        private readonly IAdminOrderService _adminOrderService;
        private readonly ILogger<AdminOrderController> _logger;

        public AdminOrderController(IAdminOrderService adminOrderService, ILogger<AdminOrderController> logger)
        {
            _adminOrderService = adminOrderService;
            _logger = logger;
        }

        // GET: AdminOrder/Details/5
        public async Task<IActionResult> Details(int id)
        {
            _logger.LogInformation("Отримання деталей замовлення з Id={OrderId}", id);

            var order = await _adminOrderService.GetOrderByIdAsync(id);
            if (order == null)
            {
                _logger.LogWarning("Замовлення з Id={OrderId} не знайдено", id);
                return NotFound();
            }

            // Підготувати список статусів для селекта
            var statuses = new List<SelectListItem>
            {
                new SelectListItem("Очікують", "Pending"),
                new SelectListItem("Підтверджені", "Confirmed"),
                new SelectListItem("Виконані", "Completed"),
                new SelectListItem("Скасовані", "Cancelled"),
            };

            // Встановити вибраний статус
            foreach (var status in statuses)
            {
                status.Selected = status.Value == order.Status;
            }

            ViewBag.StatusList = statuses;

            _logger.LogInformation("Деталі замовлення з Id={OrderId} успішно отримані", id);
            return View(order);
        }

        // POST: AdminOrder/UpdateStatus
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateStatus(UpdateOrderStatusDto dto)
        {
            _logger.LogInformation("Запит на оновлення статусу замовлення Id={OrderId} на статус {NewStatus}", dto.OrderId, dto.NewStatus);

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Невірні дані для оновлення статусу замовлення Id={OrderId}", dto.OrderId);
                TempData["Error"] = "Невірні дані для оновлення статусу.";
                return RedirectToAction(nameof(Details), new { id = dto.OrderId });
            }

            try
            {
                var success = await _adminOrderService.UpdateOrderStatusAsync(dto);
                if (!success)
                {
                    _logger.LogWarning("Не вдалося оновити статус замовлення Id={OrderId}", dto.OrderId);
                    TempData["Error"] = "Не вдалося оновити статус замовлення.";
                }
                else
                {
                    _logger.LogInformation("Статус замовлення Id={OrderId} успішно оновлено на {NewStatus}", dto.OrderId, dto.NewStatus);
                    TempData["Success"] = "Статус замовлення успішно оновлено.";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Помилка при оновленні статусу замовлення Id={OrderId}", dto.OrderId);
                TempData["Error"] = "Сталася помилка при оновленні статусу.";
            }

            return RedirectToAction(nameof(Details), new { id = dto.OrderId });
        }
    }
}
