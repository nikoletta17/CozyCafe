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
