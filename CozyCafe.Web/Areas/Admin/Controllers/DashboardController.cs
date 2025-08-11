using System;
using CozyCafe.Application.Interfaces.ForServices.ForAdmin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CozyCafe.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Admin")]
    [Route("Admin/[controller]/[action]")]
    public class DashboardController : Controller
    {
        private readonly IDashboardService _dashboardService;
        private readonly ILogger<DashboardController> _logger;

        public DashboardController(IDashboardService dashboardService, ILogger<DashboardController> logger)
        {
            _dashboardService = dashboardService;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            _logger.LogInformation("Запит статистики для адмінської панелі");

            try
            {
                var stats = await _dashboardService.GetStatsAsync();
                _logger.LogInformation("Статистика успішно отримана");
                return View(stats);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Помилка при отриманні статистики адмінської панелі");
                return StatusCode(500, "Внутрішня помилка сервера");
            }
        }
    }
}
