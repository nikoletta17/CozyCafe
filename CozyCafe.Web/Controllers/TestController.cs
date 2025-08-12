using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CozyCafe.Web.Controllers
{
    public class TestController : Controller
    {
        public IActionResult Throw500()
        {
            throw new Exception("Тестовая ошибка 500");
        }

        [Authorize]
        public IActionResult Unauthorized401()
        {
            return Content("Если ты это видишь, значит авторизация прошла.");
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Forbidden403()
        {
            return Content("Если ты это видишь, значит у тебя есть роль Admin.");
        }

        public IActionResult NotFound404()
        {
            return NotFound();
        }
    }
}
