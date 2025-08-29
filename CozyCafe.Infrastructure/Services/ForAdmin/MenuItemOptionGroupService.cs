using CozyCafe.Application.Exceptions;
using CozyCafe.Application.Interfaces.ForRerository.ForAdmin;
using CozyCafe.Application.Interfaces.ForServices.ForAdmin;
using CozyCafe.Application.Interfaces.Logging;
using CozyCafe.Application.Services.Generic_Service;
using CozyCafe.Models.Domain.Admin;
using Microsoft.Extensions.Caching.Memory;

/// <summary>
/// (UA) Сервіс для роботи з групами опцій меню у адміністративній частині CozyCafe.  
/// Наслідує базовий сервіс `Service<MenuItemOptionGroup>` та додає кешування і логування:  
/// - Отримання всіх груп з опціями з кешем (MemoryCache).  
/// - Отримання груп за конкретним меню Id з кешем.  
/// - Очищення кешу при додаванні, оновленні або видаленні групи.  
/// - Логування усіх операцій та дій з кешем.  
/// Використовується для підвищення продуктивності та підтримки актуальності даних.
///
/// (EN) Service for managing menu item option groups in the CozyCafe admin area.  
/// Inherits the base `Service<MenuItemOptionGroup>` and adds caching and logging:  
/// - Retrieve all groups with options using cache (MemoryCache).  
/// - Retrieve groups by specific menu Id using cache.  
/// - Clear cache when adding, updating, or deleting a group.  
/// - Logs all operations and cache actions.  
/// Improves performance and ensures data consistency.
/// </summary>
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

            if (!groups.Any())
            {
                _logger.LogWarning("Групи опцій меню не знайдено.");
                throw new NotFoundException("Menu item option groups", "all");
            }

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

            // Якщо немає жодної групи — просто лог і пустий список, без винятку
            if (!groups.Any())
            {
                _logger.LogInfo($"Для MenuItem Id={menuItemId} немає груп опцій. Повертаємо пустий список.");
                groups = Enumerable.Empty<MenuItemOptionGroup>();
            }

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
