using Microsoft.AspNetCore.Mvc;
using CozyCafe.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace CozyCafe.Web.Areas.User.Controllers
{
    [Area("User")]
    [Route("User/[controller]/[action]")]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<AccountController> _logger;

        public AccountController(UserManager<ApplicationUser> userManager,
                                 SignInManager<ApplicationUser> signInManager,
                                 ILogger<AccountController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        // GET: Register
        [AllowAnonymous]
        [HttpGet]
        public IActionResult Register()
        {
            _logger.LogInformation("Відкрита сторінка реєстрації користувача");
            return View();
        }

        // POST: Register
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Помилка валідації при реєстрації користувача");
                return View(model);
            }

            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                FullName = model.FullName
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                _logger.LogInformation("Користувач {UserEmail} успішно створений", model.Email);
                await _userManager.AddToRoleAsync(user, "User");
                await _signInManager.SignInAsync(user, isPersistent: false);

                var roles = await _userManager.GetRolesAsync(user);
                if (roles.Contains("Admin"))
                {
                    _logger.LogInformation("Користувач {UserEmail} має роль Admin. Перенаправлення до адмінки.", model.Email);
                    return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
                }
                else
                {
                    _logger.LogInformation("Користувач {UserEmail} має роль User. Перенаправлення до користувацької частини.", model.Email);
                    return RedirectToAction("Index", "Home", new { area = "User" });
                }
            }

            foreach (var err in result.Errors)
            {
                _logger.LogWarning("Помилка створення користувача {UserEmail}: {Error}", model.Email, err.Description);
                ModelState.AddModelError(string.Empty, err.Description);
            }

            return View(model);
        }

        // GET: Login
        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login()
        {
            _logger.LogInformation("Відкрита сторінка логіну");
            return View();
        }

        // POST: Login
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Помилка валідації при логіні користувача {UserEmail}", model.Email);
                return View(model);
            }

            var result = await _signInManager.PasswordSignInAsync(
                model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                _logger.LogInformation("Користувач {UserEmail} успішно увійшов у систему", model.Email);
                // Тут можна додати логіку ролей, але у тебе і так перенаправлення однакове
                return RedirectToAction("Index", "Home", new { area = "User" });
            }

            _logger.LogWarning("Неуспішна спроба логіну користувача {UserEmail}", model.Email);
            ModelState.AddModelError("", "Invalid login attempt");
            return View(model);
        }

        // POST: Logout
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            var userName = User.Identity?.Name ?? "Unknown";
            await _signInManager.SignOutAsync();
            _logger.LogInformation("Користувач {UserName} вийшов із системи", userName);
            return RedirectToAction("Index", "Home", new { area = "User" });
        }
    }
}
