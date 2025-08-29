using AutoMapper;
using CozyCafe.Models.Domain.Admin;
using CozyCafe.Web.Areas.User.Controllers.Generic_Controller;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace CozyCafe.Web.Controllers
{
    /// <summary>
    /// (UA) Контролер для управління опціями меню в адмінській панелі.
    /// - Наслідує GenericController для базових CRUD-операцій.
    /// - ByGroup(int groupId): отримує опції меню за конкретною групою, використовує AutoMapper для перетворення у DTO, обробляє помилки і логування.
    /// 
    /// (EN) Controller for managing menu item options in the admin panel.
    /// - Inherits from GenericController for basic CRUD operations.
    /// - ByGroup(int groupId): retrieves menu options by a specific group, uses AutoMapper to map to DTOs, handles exceptions and logging.
    /// </summary>

    [Authorize(Roles = "Admin")]
    [Area("Admin")]
    [Route("Admin/[controller]/[action]")]
    public class MenuItemOptionController : GenericController<MenuItemOption>
    {
        private readonly IMenuItemOptionService _menuItemOptionService;
        private readonly IMapper _mapper;
        private readonly ILogger<MenuItemOptionController> _logger;

        public MenuItemOptionController(IMenuItemOptionService menuItemOptionService, IMapper mapper, ILogger<MenuItemOptionController> logger)
            : base(menuItemOptionService)
        {
            _menuItemOptionService = menuItemOptionService;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IActionResult> ByGroup(int groupId)
        {
            _logger.LogInformation("Отримання опцій меню за групою Id={GroupId}", groupId);

            try
            {
                var options = await _menuItemOptionService.GetByGroupIdAsync(groupId);
                var dtos = _mapper.Map<IEnumerable<MenuItemOptionDto>>(options);
                _logger.LogInformation("Опції меню за групою Id={GroupId} успішно отримані", groupId);
                return View("Index", dtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Помилка при отриманні опцій меню за групою Id={GroupId}", groupId);
                return StatusCode(500, "Внутрішня помилка сервера");
            }
        }
    }
}
