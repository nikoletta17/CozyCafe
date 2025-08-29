using AutoMapper;
using CozyCafe.Models.Domain.Admin;
using CozyCafe.Web.Areas.User.Controllers.Generic_Controller;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CozyCafe.Web.Controllers
{
    /// <summary>
    /// (UA) Контролер для управління групами опцій меню в адмінській панелі.
    /// - Наслідує GenericController для базових CRUD-операцій.
    /// - WithOptions(): отримує всі групи опцій разом з опціями.
    /// - ByMenuItem(int menuItemId): отримує групи опцій для конкретного меню.
    /// - Використовує AutoMapper для DTO та логування помилок.
    /// 
    /// (EN) Controller for managing menu item option groups in the admin panel.
    /// - Inherits from GenericController for basic CRUD operations.
    /// - WithOptions(): retrieves all option groups with their options.
    /// - ByMenuItem(int menuItemId): retrieves option groups for a specific menu item.
    /// - Uses AutoMapper for DTO mapping and exception logging.
    /// </summary>

    [Authorize(Roles = "Admin")]
    [Area("Admin")]
    [Route("Admin/[controller]/[action]")]
    public class MenuItemOptionGroupController : GenericController<MenuItemOptionGroup>
    {
        private readonly IMenuItemOptionGroupService _menuItemOptionGroupService;
        private readonly IMapper _mapper;
        private readonly ILogger<MenuItemOptionGroupController> _logger;

        public MenuItemOptionGroupController(IMenuItemOptionGroupService menuItemOptionGroupService, IMapper mapper, ILogger<MenuItemOptionGroupController> logger)
            : base(menuItemOptionGroupService)
        {
            _menuItemOptionGroupService = menuItemOptionGroupService;
            _mapper = mapper;
            _logger = logger;
        }

        // GET: MenuItemOptionGroup/WithOptions
        public async Task<IActionResult> WithOptions()
        {
            _logger.LogInformation("Отримання усіх груп опцій меню з їх опціями");

            try
            {
                var groups = await _menuItemOptionGroupService.GetAllWithOptionsAsync();
                var dtos = _mapper.Map<IEnumerable<MenuItemOptionGroup>>(groups);
                _logger.LogInformation("Групи опцій меню успішно отримані");
                return View("Index", dtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Помилка при отриманні груп опцій меню");
                return StatusCode(500, "Внутрішня помилка сервера");
            }
        }

        // GET: MenuItemOptionGroup/ByMenuItem/id
        public async Task<IActionResult> ByMenuItem(int menuItemId)
        {
            _logger.LogInformation("Отримання груп опцій меню для меню Id={MenuItemId}", menuItemId);

            try
            {
                var groups = await _menuItemOptionGroupService.GetByMenuItemIdAsync(menuItemId);
                var dtos = _mapper.Map<IEnumerable<MenuItemOptionGroup>>(groups);
                _logger.LogInformation("Групи опцій меню для меню Id={MenuItemId} успішно отримані", menuItemId);
                return View("Index", dtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Помилка при отриманні груп опцій меню для меню Id={MenuItemId}", menuItemId);
                return StatusCode(500, "Внутрішня помилка сервера");
            }
        }
    }
}
