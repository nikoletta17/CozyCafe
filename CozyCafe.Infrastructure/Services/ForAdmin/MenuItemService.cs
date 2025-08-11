using CozyCafe.Application.Interfaces.ForRerository.ForAdmin;
using CozyCafe.Application.Interfaces.ForServices.ForAdmin;
using CozyCafe.Application.Services.Generic_Service;
using CozyCafe.Models.Domain.Admin;
using CozyCafe.Models.DTO.Admin;
using Microsoft.Extensions.Logging;

public class MenuItemService : Service<MenuItem>, IMenuItemService
{
    private readonly IMenuItemRepository _menuItemRepository;
    private readonly ILogger<MenuItemService> _logger;

    public MenuItemService(IMenuItemRepository menuItemRepository,
                           ILogger<MenuItemService> logger)
        : base(menuItemRepository)
    {
        _menuItemRepository = menuItemRepository;
        _logger = logger;
    }

    public async Task<IEnumerable<MenuItemDto>> GetFilteredAsync(MenuItemFilterModel filterModel)
    {
        _logger.LogInformation("Filtering MenuItems with parameters: {@FilterModel}", filterModel);
        var items = await _menuItemRepository.GetFilteredAsync(filterModel);
        _logger.LogInformation("Found {Count} filtered MenuItems", items.Count());

        return items.Select(mi => new MenuItemDto
        {
            Id = mi.Id,
            Name = mi.Name,
            Description = mi.Description,
            Price = mi.Price,
            ImageUrl = mi.ImageUrl,
            CategoryName = mi.Category?.Name ?? ""
        });
    }

    public async Task<MenuItemDto?> GetByIdAsync(int id)
    {
        _logger.LogInformation("Fetching MenuItem by Id={Id}", id);
        var item = await _menuItemRepository.GetByIdWithCategoryAsync(id);
        if (item == null)
        {
            _logger.LogWarning("MenuItem with Id={Id} not found", id);
            return null;
        }

        _logger.LogInformation("MenuItem found: {Name}", item.Name);
        return new MenuItemDto
        {
            Id = item.Id,
            Name = item.Name,
            Description = item.Description,
            Price = item.Price,
            ImageUrl = item.ImageUrl,
            CategoryName = item.Category?.Name ?? "Без категорії"
        };
    }
}
