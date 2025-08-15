using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CozyCafe.Web.Controllers
{
    public class TestController : Controller
    {
        public IActionResult Throw500()
        {
            throw new Exception("Тестова помилка 500");
        }

        [Authorize]
        public IActionResult Unauthorized401()
        {
            return Content("Авторизація пройшла успішно.");
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Forbidden403()
        {
            return Content("Вам присвоєно роль Адміністратора");
        }

        public IActionResult NotFound404()
        {
            return NotFound();
        }
    }
}
