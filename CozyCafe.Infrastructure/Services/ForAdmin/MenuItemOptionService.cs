using CozyCafe.Application.Exceptions;
using CozyCafe.Application.Interfaces.ForRerository.ForAdmin;
using CozyCafe.Application.Interfaces.ForServices.ForAdmin;
using CozyCafe.Application.Services.Generic_Service;
using CozyCafe.Models.Domain.Admin;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

public class MenuItemOptionService : Service<MenuItemOption>, IMenuItemOptionService
{
    private readonly IMenuItemOptionRepository _menuItemOptionRepository;
    private readonly ILogger<MenuItemOptionService> _logger;
    private readonly IMemoryCache _cache;

    private const string OptionsByGroupCacheKeyPrefix = "MenuItemOptions_Group_";
    private const string OptionsByIdsCacheKeyPrefix = "MenuItemOptions_Ids_";

    public MenuItemOptionService(
        IMenuItemOptionRepository menuItemOptionRepository,
        ILogger<MenuItemOptionService> logger,
        IMemoryCache cache)
        : base(menuItemOptionRepository)
    {
        _menuItemOptionRepository = menuItemOptionRepository;
        _logger = logger;
        _cache = cache;
    }

    public async Task<IEnumerable<MenuItemOption>> GetByGroupIdAsync(int groupId)
    {
        string cacheKey = $"{OptionsByGroupCacheKeyPrefix}{groupId}";

        if (!_cache.TryGetValue(cacheKey, out IEnumerable<MenuItemOption> options))
        {
            _logger.LogInformation($"[CACHE MISS] Отримання опцій групи Id={groupId}");
            options = await _menuItemOptionRepository.GetByGroupIdAsync(groupId);

            if (!options.Any())
            {
                _logger.LogWarning($"Опцій для групи Id={groupId} не знайдено.");
                throw new NotFoundException("Menu item options group", groupId);
            }

            _cache.Set(cacheKey, options, TimeSpan.FromMinutes(10));
            _logger.LogInformation($"[CACHE SET] Збережено {options.Count()} опцій групи Id={groupId} у кеш.");
        }
        else
        {
            _logger.LogInformation($"[CACHE HIT] Отримано опції групи Id={groupId} з кешу.");
        }
        return options;
    }

    public async Task<IEnumerable<MenuItemOption>> GetByIdsAsync(IEnumerable<int> ids)
    {
        string idsKeyPart = string.Join("_", ids.OrderBy(i => i));
        string cacheKey = $"{OptionsByIdsCacheKeyPrefix}{idsKeyPart}";

        if (!_cache.TryGetValue(cacheKey, out IEnumerable<MenuItemOption> options))
        {
            _logger.LogInformation($"[CACHE MISS] Отримання опцій за IDs: {idsKeyPart}");
            options = await _menuItemOptionRepository.GetByIdsAsync(ids);

            if (!options.Any())
            {
                _logger.LogWarning($"Опцій з IDs {idsKeyPart} не знайдено.");
                throw new NotFoundException("Menu item options", idsKeyPart);
            }

            _cache.Set(cacheKey, options, TimeSpan.FromMinutes(10));
            _logger.LogInformation($"[CACHE SET] Збережено {options.Count()} опцій за IDs у кеш.");
        }
        else
        {
            _logger.LogInformation($"[CACHE HIT] Отримано опції за IDs з кешу.");
        }
        return options;
    }

    public override async Task AddAsync(MenuItemOption entity)
    {
        await base.AddAsync(entity);
        ClearCache();
    }

    public override async Task UpdateAsync(MenuItemOption entity)
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
        _logger.LogInformation("[CACHE CLEAR] Очищення кешу опцій меню.");
    }
}
