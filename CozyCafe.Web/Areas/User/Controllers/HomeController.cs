using CozyCafe.Application.Interfaces.ForServices.ForAdmin;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;
using CozyCafe.Models.Domain.Admin;
using CozyCafe.Web.Models;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using CozyCafe.Application.Interfaces.Logging;


namespace CozyCafe.Web.Areas.User.Controllers
{
    /// <summary>
    /// (UA) Головний контролер користувацької частини.
    /// - Index(): Показує всі категорії на головній сторінці.
    /// - Privacy(): Показує сторінку політики конфіденційності.
    /// - Error(): Відображає сторінку помилок із RequestId.
    /// - Використовує ILoggerService та ICategoryService для логування та отримання даних.
    /// 
    /// (EN) Main controller for the User area.
    /// - Index(): Displays all categories on the home page.
    /// - Privacy(): Shows privacy policy page.
    /// - Error(): Displays error page with RequestId.
    /// - Uses ILoggerService and ICategoryService for logging and data retrieval.
    /// </summary>

    [Area("User")]
    [Route("User/[controller]/[action]")]
    public class HomeController : Controller
    {
        private readonly ILoggerService _logger;
        private readonly ICategoryService _categoryService;

        public HomeController(ILoggerService logger, ICategoryService categoryService)
        {
            _logger = logger;
            _categoryService = categoryService;
        }

        public async Task<IActionResult> Index()
        {
            _logger.LogInfo("Це тестовий лог в Index");
            IEnumerable<Category> categories = await _categoryService.GetAllAsync();
            return View(categories);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
