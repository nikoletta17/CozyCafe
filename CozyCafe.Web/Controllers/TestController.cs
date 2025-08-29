using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CozyCafe.Web.Controllers
{
    /// <summary>
    /// (UA) Контролер для тестування різних сценаріїв помилок та авторизації.
    /// - Throw500(): штучно викликає помилку 500 для тестування.
    /// - Unauthorized401(): приклад захищеного методу, доступного лише авторизованим користувачам.
    /// - Forbidden403(): приклад методу з обмеженням по ролі Admin.
    /// - NotFound404(): повертає 404 Not Found для тестування.
    /// 
    /// (EN) Controller for testing various error and authorization scenarios.
    /// - Throw500(): artificially throws a 500 error for testing purposes.
    /// - Unauthorized401(): example of a method accessible only to authorized users.
    /// - Forbidden403(): example of a method restricted to the Admin role.
    /// - NotFound404(): returns 404 Not Found for testing purposes.
    /// </summary>

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
