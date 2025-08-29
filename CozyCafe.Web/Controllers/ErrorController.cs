using CozyCafe.Web.Models;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace CozyCafe.Web.Controllers
{
    /// <summary>
    /// (UA) Контролер для обробки помилок у CozyCafe.
    /// - Метод Error(): обробляє винятки сервера (500 та інші) та передає повідомлення у View.
    /// - Метод HttpStatusCodeHandler(int statusCode): обробляє інші HTTP-коди помилок (404, 403, 401 тощо) з кастомними повідомленнями.
    /// Використовує ErrorViewModel для відображення інформації про помилку.
    /// 
    /// (EN) Controller for handling errors in CozyCafe.
    /// - Error(): handles server exceptions (500 etc.) and passes the message to the View.
    /// - HttpStatusCodeHandler(int statusCode): handles other HTTP error codes (404, 403, 401, etc.) with custom messages.
    /// Uses ErrorViewModel to display error details.
    /// </summary>

    public class ErrorController : Controller
    {
        // Обработка 500 и других исключений
        [Route("Error")]
        public IActionResult Error()
        {
            var exceptionHandlerFeature = HttpContext.Features.Get<IExceptionHandlerFeature>();

            var model = new ErrorViewModel
            {
                RequestId = HttpContext.TraceIdentifier
            };

            ViewBag.StatusCode = 500;

            if (exceptionHandlerFeature != null)
            {
                ViewBag.ErrorMessage = exceptionHandlerFeature.Error.Message;
            }

            Response.StatusCode = 500;
            return View("Error", model);
        }

        // Обработка всех остальных кодов ошибок
        [Route("Error/{statusCode}")]
        public IActionResult HttpStatusCodeHandler(int statusCode)
        {
            var model = new ErrorViewModel
            {
                RequestId = HttpContext.TraceIdentifier
            };

            ViewBag.StatusCode = statusCode;

            // Для некоторых кодов можно задать кастомные сообщения
            switch (statusCode)
            {
                case 404:
                    ViewBag.ErrorMessage = "Сторінка не знайдена.";
                    break;
                case 403:
                    ViewBag.ErrorMessage = "У вас немає прав доступу до цієї сторінки.";
                    break;
                case 401:
                    ViewBag.ErrorMessage = "Ви не авторизовані для перегляду цієї сторінки.";
                    break;
                default:
                    ViewBag.ErrorMessage = "Сталася помилка.";
                    break;
            }

            Response.StatusCode = statusCode;
            return View("Error", model);
        }
    }
}
