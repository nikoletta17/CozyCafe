using CozyCafe.Application.Interfaces.ForServices.ForAdmin;
using CozyCafe.Models.DTO.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

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

        // GET: AdminOrder/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var order = await _adminOrderService.GetOrderByIdAsync(id);
            if (order == null)
                return NotFound();

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
                return RedirectToAction(nameof(Details), new { id = dto.OrderId });
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
            catch
            {
                TempData["Error"] = "Сталася помилка при оновленні статусу.";
            }

            return RedirectToAction(nameof(Details), new { id = dto.OrderId });
        }
    }
}
