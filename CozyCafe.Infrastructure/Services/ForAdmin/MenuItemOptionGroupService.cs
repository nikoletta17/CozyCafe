using CozyCafe.Application.Interfaces.ForRerository.ForAdmin;
using CozyCafe.Application.Interfaces.ForServices.ForAdmin;
using CozyCafe.Application.Interfaces.Logging;
using CozyCafe.Application.Services.Generic_Service;
using CozyCafe.Models.Domain.Admin;
using Microsoft.Extensions.Caching.Memory;

public class MenuItemOptionGroupService : Service<MenuItemOptionGroup>, IMenuItemOptionGroupService
{
    private readonly IMenuItemOptionGroupRepository _menuItemOptionGroupRepository;
    private readonly ILoggerService _logger;
    private readonly IMemoryCache _cache;

    private const string AllGroupsCacheKey = "MenuItemOptionGroups_All";
    private const string GroupsByMenuItemCacheKeyPrefix = "MenuItemOptionGroups_MenuItem_";

    public MenuItemOptionGroupService(
        IMenuItemOptionGroupRepository menuItemOptionGroupRepository,
        ILoggerService logger,
        IMemoryCache cache)
        : base(menuItemOptionGroupRepository)
    {
        _menuItemOptionGroupRepository = menuItemOptionGroupRepository;
        _logger = logger;
        _cache = cache;
    }

    public async Task<IEnumerable<MenuItemOptionGroup>> GetAllWithOptionsAsync()
    {
        if (!_cache.TryGetValue(AllGroupsCacheKey, out IEnumerable<MenuItemOptionGroup> groups))
        {
            _logger.LogInfo("[CACHE MISS] Отримання всіх груп опцій меню з опціями.");
            groups = await _menuItemOptionGroupRepository.GetAllWithOptionsAsync();

            _cache.Set(AllGroupsCacheKey, groups, TimeSpan.FromMinutes(10));
            _logger.LogInfo($"[CACHE SET] Збережено {groups.Count()} груп опцій у кеш.");
        }
        else
        {
            _logger.LogInfo("[CACHE HIT] Отримано всі групи опцій з кешу.");
        }
        return groups;
    }

    public async Task<IEnumerable<MenuItemOptionGroup>> GetByMenuItemIdAsync(int menuItemId)
    {
        string cacheKey = $"{GroupsByMenuItemCacheKeyPrefix}{menuItemId}";

        if (!_cache.TryGetValue(cacheKey, out IEnumerable<MenuItemOptionGroup> groups))
        {
            _logger.LogInfo($"[CACHE MISS] Отримання груп опцій для меню Id={menuItemId}.");
            groups = await _menuItemOptionGroupRepository.GetByMenuItemIdAsync(menuItemId);

            _cache.Set(cacheKey, groups, TimeSpan.FromMinutes(10));
            _logger.LogInfo($"[CACHE SET] Збережено {groups.Count()} груп опцій для меню Id={menuItemId} у кеш.");
        }
        else
        {
            _logger.LogInfo($"[CACHE HIT] Отримано групи опцій для меню Id={menuItemId} з кешу.");
        }
        return groups;
    }

    public override async Task AddAsync(MenuItemOptionGroup entity)
    {
        await base.AddAsync(entity);
        ClearCache();
    }

    public override async Task UpdateAsync(MenuItemOptionGroup entity)
    {
        await base.UpdateAsync(entity);
        ClearCache();
    }

    public override async Task DeleteAsync(int id)
    {
        await base.DeleteAsync(id);
        ClearCache();
    }

    private void ClearCache()
    {
        _logger.LogInfo("[CACHE CLEAR] Очищення кешу груп опцій меню.");
        _cache.Remove(AllGroupsCacheKey);
    }
}
