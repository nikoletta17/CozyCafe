using CozyCafe.Application.Interfaces.ForRerository.ForAdmin;
using CozyCafe.Application.Interfaces.ForServices.ForAdmin;
using CozyCafe.Application.Services.Generic_Service;
using CozyCafe.Models.Domain.Admin;
using Microsoft.Extensions.Logging;

public class MenuItemOptionService : Service<MenuItemOption>, IMenuItemOptionService
{
    private readonly IMenuItemOptionRepository _menuItemOptionRepository;
    private readonly ILogger<MenuItemOptionService> _logger;

    public MenuItemOptionService(IMenuItemOptionRepository menuItemOptionRepository,
                                 ILogger<MenuItemOptionService> logger)
        : base(menuItemOptionRepository)
    {
        _menuItemOptionRepository = menuItemOptionRepository;
        _logger = logger;
    }

    public async Task<IEnumerable<MenuItemOption>> GetByGroupIdAsync(int groupId)
    {
        _logger.LogInformation("Fetching MenuItemOptions for GroupId={GroupId}", groupId);
        var result = await _menuItemOptionRepository.GetByGroupIdAsync(groupId);
        _logger.LogInformation("Found {Count} MenuItemOptions for GroupId={GroupId}", result.Count(), groupId);
        return result;
    }

    public async Task<IEnumerable<MenuItemOption>> GetByIdsAsync(IEnumerable<int> ids)
    {
        _logger.LogInformation("Fetching MenuItemOptions by IDs: {Ids}", string.Join(", ", ids));
        var result = await _menuItemOptionRepository.GetByIdsAsync(ids);
        _logger.LogInformation("Found {Count} MenuItemOptions for provided IDs", result.Count());
        return result;
    }
}
