using CozyCafe.Application.Interfaces.ForRerository.ForAdmin;
using CozyCafe.Application.Interfaces.ForServices.ForAdmin;
using CozyCafe.Application.Interfaces.Logging;
using CozyCafe.Application.Services.Generic_Service;
using CozyCafe.Models.Domain.Admin;

public class MenuItemOptionGroupService : Service<MenuItemOptionGroup>, IMenuItemOptionGroupService
{
    private readonly IMenuItemOptionGroupRepository _menuItemOptionGroupRepository;
    private readonly ILoggerService _logger;

    public MenuItemOptionGroupService(IMenuItemOptionGroupRepository menuItemOptionGroupRepository, ILoggerService logger)
        : base(menuItemOptionGroupRepository)
    {
        _menuItemOptionGroupRepository = menuItemOptionGroupRepository;
        _logger = logger;
    }

    public async Task<IEnumerable<MenuItemOptionGroup>> GetAllWithOptionsAsync()
    {
        _logger.LogInfo("Отримання всіх груп опцій меню з їх опціями.");
        var result = await _menuItemOptionGroupRepository.GetAllWithOptionsAsync();
        _logger.LogInfo($"Знайдено {result.Count()} груп опцій.");
        return result;
    }

    public async Task<IEnumerable<MenuItemOptionGroup>> GetByMenuItemIdAsync(int menuItemId)
    {
        _logger.LogInfo($"Отримання груп опцій для меню ID={menuItemId}.");
        var result = await _menuItemOptionGroupRepository.GetByMenuItemIdAsync(menuItemId);
        _logger.LogInfo($"Знайдено {result.Count()} груп опцій для меню ID={menuItemId}.");
        return result;
    }
}
