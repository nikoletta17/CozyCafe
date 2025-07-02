using Microsoft.AspNetCore.Mvc;

namespace CozyCafe.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return RedirectToAction("Index", "Home", new { area = "User" });
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}
