using CozyCafe.Application.Interfaces.Logging;
using CozyCafe.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CozyCafe.Web.Areas.User.Controllers
{
    [Area("User")]
    [Authorize]
    [Route("User/[controller]/[action]")]
    public class UserProfileController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILoggerService _logger;

        public UserProfileController(UserManager<ApplicationUser> userManager,
                                     SignInManager<ApplicationUser> signInManager,
                                     ILoggerService logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        // --- Редагування профілю ---

        [HttpGet]
        public async Task<IActionResult> Edit()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                _logger.LogWarning("UserProfileController.Edit: Незареєстрований користувач намагався зайти");
                return Unauthorized();
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                _logger.LogWarning($"UserProfileController.Edit: Користувача з Id {userId} не знайдено");
                return NotFound();
            }

            var model = new UserProfileViewModel
            {
                FullName = user.FullName,
                Email = user.Email,
               
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UserProfileViewModel model)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning($"UserProfileController.Edit: Некоректна модель при оновленні профілю користувача {User.Identity?.Name}");
                return View(model);
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                _logger.LogWarning("UserProfileController.Edit POST: Незареєстрований користувач");
                return Unauthorized();
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                _logger.LogWarning($"UserProfileController.Edit POST: Користувача з Id {userId} не знайдено");
                return NotFound();
            }

            user.FullName = model.FullName;
            user.Email = model.Email;
            user.UserName = model.Email; // якщо логін - Email

            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
            {
                foreach (var error in updateResult.Errors)
                    ModelState.AddModelError("", error.Description);

                _logger.LogWarning($"UserProfileController.Edit POST: Помилка оновлення профілю користувача {User.Identity?.Name}");
                return View(model);
            }

            _logger.LogInfo($"{User.Identity?.Name}: Успішне оновлення профілю");
            TempData["Success"] = "Профіль успішно оновлено.";
            return RedirectToAction(nameof(Edit));
        }

        // --- Зміна пароля ---

        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning($"{User.Identity?.Name}: Некоректні дані при зміні пароля");
                return View(model);
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                _logger.LogWarning($"UserProfileController.ChangePassword POST: Користувача з Id {userId} не знайдено");
                return Unauthorized();
            }

            var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError("", error.Description);

                _logger.LogWarning($"{User.Identity?.Name}: Помилка зміни пароля");
                return View(model);
            }

            await _signInManager.RefreshSignInAsync(user);
            _logger.LogInfo($"{User.Identity?.Name}: Успішна зміна пароля");
            TempData["Success"] = "Пароль успішно змінено.";
            return RedirectToAction(nameof(Edit));
        }

        // --- Видалення акаунту ---

        [HttpGet]
        public IActionResult DeleteAccount()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAccount(DeleteAccountViewModel model)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning($"{User.Identity?.Name}: Некоректні дані при видаленні акаунту");
                return View(model);
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                _logger.LogWarning($"UserProfileController.DeleteAccount POST: Користувача з Id {userId} не знайдено");
                return Unauthorized();
            }

            var passwordValid = await _userManager.CheckPasswordAsync(user, model.Password);
            if (!passwordValid)
            {
                ModelState.AddModelError("", "Неправильний пароль.");
                _logger.LogWarning($"{User.Identity?.Name}: Спроба видалення акаунту з неправильним паролем");
                return View(model);
            }

            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Помилка при видаленні акаунту.");
                _logger.LogWarning($"{User.Identity?.Name}: Помилка видалення акаунту");
                return View(model);
            }

            await _signInManager.SignOutAsync();
            _logger.LogInfo($"{User.Identity?.Name}: Акаунт видалено");
            return RedirectToAction("Index", "Home");
        }
    }
}
